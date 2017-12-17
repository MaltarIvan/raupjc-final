using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Core
{
    public class UserProfile
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public List<Album> Albums { get; set; }
        public List<Picture> Favorites { get; set; }
        public DateTime DateCreated { get; set; }
        public Picture ProfilePicture { get; set; }

        public UserProfile()
        {
        }

        public UserProfile(Guid id)
        {
            Id = id;
            DateCreated = DateTime.Now;
        }
    }
}
