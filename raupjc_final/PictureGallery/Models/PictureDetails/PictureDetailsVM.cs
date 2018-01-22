using PictureGallery.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.PictureDetails
{
    public class PictureDetailsVM
    {
        public Guid Id { get; set; }
        public UserProfile User { get; set; }
        public Album Album { get; set; }
        public string Url { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfDislikes { get; set; }
        public string DateCreated { get; set; }
        public string Description { get; set; }
        public List<CommentVM> CommentsVM { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsUsersPicture { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsHot { get; set; }

        public PictureDetailsVM(bool isFollowing, Guid currentUserId, bool isAdmin, Picture picture)
        {
            Id = picture.Id;
            User = picture.User;
            Album = picture.Album;
            Url = picture.Url;
            NumberOfLikes = picture.NumberOfLikes;
            NumberOfDislikes = picture.NumberOfDislikes;
            DateCreated = picture.DateCreted.ToShortDateString();
            Description = picture.Description;
            CommentsVM = new List<CommentVM>();
            foreach (var com in picture.Comments)
            {
                CommentsVM.Add(new CommentVM(currentUserId == com.User.Id, com));
            }
            IsFollowing = isFollowing;
            IsUsersPicture = currentUserId == picture.UserId;
            IsAdmin = isAdmin;
            IsHot = picture.IsHot;
        }
    }
}
