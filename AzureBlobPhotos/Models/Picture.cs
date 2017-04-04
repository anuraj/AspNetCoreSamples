using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AzureBlobPhotos.Models
{
    public class Picture
    {
        [Required(ErrorMessage = "File is required")]
        [FileExtensions(ErrorMessage = "Please select a valid format")]
        public IFormFile File { get; set; }
    }
}