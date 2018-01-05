using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Web.UI.WebControls;

namespace PictureGallery.Models
{
    public class AddUserProfileVM
    {
        [Required, MinLength(3)]
        public string UserName { get; set; }
        public IFormFile ProfilePictureUpload { get; set; }
    }
}
