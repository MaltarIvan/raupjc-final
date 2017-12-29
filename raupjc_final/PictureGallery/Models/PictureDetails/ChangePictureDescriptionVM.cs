using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.PictureDetails
{
    public class ChangePictureDescriptionVM
    {
        [Required]
        public string Description { get; set; }
        public Guid Id { get; set; }
    }
}
