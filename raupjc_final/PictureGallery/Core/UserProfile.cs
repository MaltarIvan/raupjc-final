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
        public List<Picture> PicturesLiked { get; set; }
        public List<Picture> PicturesDisliked { get; set; }
        public List<Comment> Comments { get; set; }

        public UserProfile()
        {
            Albums = new List<Album>();
            Favorites = new List<Picture>();
            PicturesLiked = new List<Picture>();
            PicturesDisliked = new List<Picture>();
            Comments = new List<Comment>();
        }

        public UserProfile(Guid id)
        {
            Id = id;
            DateCreated = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            UserProfile userProfile = (UserProfile)obj;
            return Id == userProfile.Id;
        }
    }
}
