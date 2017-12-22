using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Core
{
    public class Comment
    {
        public Guid Id { get; set; }
        public UserProfile User { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public Picture Picture { get; set; }

        public Comment()
        {

        }

        public Comment(UserProfile user, Picture picture, string text)
        {
            Id = Guid.NewGuid();
            User = user;
            Picture = picture;
            Text = text;
            DateCreated = DateTime.Now;
        }
    }
}
