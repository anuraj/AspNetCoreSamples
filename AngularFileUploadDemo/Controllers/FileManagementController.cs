using AngularFileUploadDemo.ViewModels;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;

using Myrmec;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AngularFileUploadDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileManagementController : ControllerBase
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload()
        {
            var sniffer = new Sniffer();
            var supportedFiles = new List<Record>
    {
        new Record("doc xls ppt msg", "D0 CF 11 E0 A1 B1 1A E1"),
        new Record("jpg,jpeg", "ff,d8,ff,db"),
        new Record("png", "89,50,4e,47,0d,0a,1a,0a"),
        new Record("pdf", "25 50 44 46"),
        new Record("gif", "47 49 46 38 39 61")
    };
            sniffer.Populate(supportedFiles);

            var formCollection = await Request.ReadFormAsync();
            var files = formCollection.Files;
            foreach (var file in files)
            {
                byte[] fileHead = ReadFileHead(file);
                var results = sniffer.Match(fileHead);
                if (results.Count > 0)
                {
                    var blobContainerClient = new BlobContainerClient("UseDevelopmentStorage=true", "images");
                    blobContainerClient.CreateIfNotExists();
                    var containerClient = blobContainerClient.GetBlobClient(file.FileName);
                    var blobHttpHeader = new BlobHttpHeaders
                    {
                        ContentType = file.ContentType
                    };
                    await containerClient.UploadAsync(file.OpenReadStream(), blobHttpHeader);
                }
            }

            return Ok();
        }

        private static byte[] ReadFileHead(IFormFile file)
        {
            using var fs = new BinaryReader(file.OpenReadStream());
            var bytes = new byte[20];
            fs.Read(bytes, 0, 20);
            return bytes;
        }

        [HttpPost]
        [Route("createprofile")]
        public async Task<IActionResult> CreateProfile([FromForm] Profile profile)
        {
            if (ModelState.IsValid)
            {
                var tempProfile = profile;
                return Ok();
            }

            return BadRequest();
        }
    }
}
