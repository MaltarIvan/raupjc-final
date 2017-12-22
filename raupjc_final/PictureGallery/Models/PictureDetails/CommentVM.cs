using PictureGallery.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.PictureDetails
{
    public class CommentVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UsersProfilePicture { get; set; }
        public string DateCreated { get; set; }
        public string Text { get; set; }

        public CommentVM(Comment comment)
        {
            Id = comment.Id;
            UserName = comment.User.UserName;
            UsersProfilePicture = Convert.ToBase64String(comment.User.ProfilePicture.Data);
            DateCreated = comment.DateCreated.ToShortDateString();
            Text = comment.Text;
        }
    }
}
