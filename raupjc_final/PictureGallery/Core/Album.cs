using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Core
{
    public class Album
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public List<Picture> Pictures { get; set; }
        public UserProfile User { get; set; }

        public Album(Guid userId, string description)
        {
            Id = new Guid();
            UserId = userId;
            DateCreated = DateTime.Now;
            Description = description;
            Pictures = new List<Picture>();
        }

        public Album()
        {
        }
    }
}
