using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.ManageAlbum
{
    public class ChangeAlbumDescriptionVM
    {
        public Guid Id { get; set; }
        [Required, MinLength(3)]
        public string Description { get; set; }
    }
}
