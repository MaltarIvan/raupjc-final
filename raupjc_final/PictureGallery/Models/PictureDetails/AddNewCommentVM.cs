using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.PictureDetails
{
    public class AddNewCommentVM
    {
        public Guid PictureId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
