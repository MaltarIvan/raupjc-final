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
        public string Url { get; set; }
        public string DateCreated { get; set; }
        public string Description { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfDislikes { get; set; }
        public int NumberOfComments { get; set; }

        public PictureVM(Picture picture)
        {
            Id = picture.Id;
            UserId = picture.UserId;
            Url = picture.Url;
            DateCreated = picture.DateCreted.ToShortDateString();
            Description = picture.Description;
            NumberOfLikes = picture.NumberOfLikes;
            NumberOfDislikes = picture.NumberOfDislikes;
            NumberOfComments = picture.Comments.Count();
        }
    }
}
