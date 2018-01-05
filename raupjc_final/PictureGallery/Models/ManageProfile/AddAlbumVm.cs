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
        [Required, MinLength(3)]
        public string Description { get; set; }
    }
}
