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
        public string Data { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfDislikes { get; set; }
        public string DateCreated { get; set; }
        public string Description { get; set; }
        public List<CommentVM> CommentsVM { get; set; }
        public bool IsFollowing { get; set; }

        public PictureDetailsVM(bool isFollowing, Picture picture)
        {
            Id = picture.Id;
            User = picture.User;
            Album = picture.Album;
            Data = Convert.ToBase64String(picture.Data);
            NumberOfLikes = picture.NumberOfLikes;
            NumberOfDislikes = picture.NumberOfDislikes;
            DateCreated = picture.DateCreted.ToShortDateString();
            Description = picture.Description;
            CommentsVM = new List<CommentVM>();
            foreach (var com in picture.Comments)
            {
                CommentsVM.Add(new CommentVM(com));
            }
            IsFollowing = isFollowing;
        }
    }
}
