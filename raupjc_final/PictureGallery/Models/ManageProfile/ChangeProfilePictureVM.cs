using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Main
{
    public class ChangeProfilePictureVM
    {
        [Required]
        public IFormFile ProfilePictureUpload { get; set; }
    }
}
