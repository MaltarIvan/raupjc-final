using PictureGallery.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Shared
{
    public class PictureVM
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Data { get; set; }
        public string DateCreated { get; set; }
        public string Description { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfDislikes { get; set; }

        public PictureVM(Picture picture)
        {
            Id = picture.Id;
            UserId = picture.UserId;
            Data = Convert.ToBase64String(picture.Data);
            DateCreated = picture.DateCreted.ToShortDateString();
            Description = picture.Description;
            NumberOfLikes = picture.NumberOfLikes;
            NumberOfDislikes = picture.NumberOfDislikes;
        }
    }
}
