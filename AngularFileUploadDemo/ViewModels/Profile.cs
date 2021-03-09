using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Table;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularFileUploadDemo.ViewModels
{
    public class Profile
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public IFormFile Picture { get; set; }
    }
}
