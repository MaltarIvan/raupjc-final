using PictureGallery.Models.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.ManageProfile
{
    public class AddAlbumVM
    {
        [Required]
        public string Description { get; set; }
    }
}
