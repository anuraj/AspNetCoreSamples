using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AzureBlobPhotos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobPhotos.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var urls = new List<string>();
            var storageAccount = CloudStorageAccount.Parse("");
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("staticcontent");
            var data = await container.ListBlobsSegmentedAsync
                ("", true, BlobListingDetails.All, 200, null, null, null);
            foreach (IListBlobItem item in data.Results)
            {
                urls.Add(item.StorageUri.PrimaryUri.ToString());
            }

            return View(urls);
        }

        public async Task<IActionResult> Upload(Picture picture)
        {
            var storageAccount = CloudStorageAccount.Parse("");
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("staticcontent");
            var file = picture.File;
            var parsedContentDisposition =
                ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = Path.Combine(parsedContentDisposition.FileName.Trim('"'));
            var blockBlob = container.GetBlockBlobReference(filename);
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            return RedirectToAction("Index");
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
