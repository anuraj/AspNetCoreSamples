using AngularFileUploadDemo.ViewModels;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;

using System;
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
            var formCollection = await Request.ReadFormAsync();
            var files = formCollection.Files;
            foreach (var file in files)
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

            return Ok();
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
