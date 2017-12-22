using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Core
{
    public class Picture
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public DateTime DateCreted { get; set; }
        public Album Album { get; set; }
        public byte[] Data { get; set; }
        public UserProfile User { get; set; }
        public List<UserProfile> UsersFavorite { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfDislikes { get; set; }
        public List<UserProfile> UsersLiked { get; set; }
        public List<UserProfile> UsersDisliked { get; set; }
        public List<Comment> Comments { get; set; } 
        
        public Picture(Guid userId, string description, Album album, byte[] data)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Description = description;
            DateCreted = DateTime.Now;
            Album = album;
            Data = data;
            UsersFavorite = new List<UserProfile>();
            NumberOfLikes = 0;
            NumberOfDislikes = 0;
            UsersLiked = new List<UserProfile>();
            UsersDisliked = new List<UserProfile>();
            Comments = new List<Comment>();
        }

        public Picture(Guid userId, byte[] data)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Data = data;
            DateCreted = DateTime.Now;
            NumberOfLikes = 0;
            NumberOfDislikes = 0;
            UsersFavorite = new List<UserProfile>();
            UsersLiked = new List<UserProfile>();
            UsersDisliked = new List<UserProfile>();
            Comments = new List<Comment>();
        }

        public Picture()
        {
            Comments = new List<Comment>();
            UsersLiked = new List<UserProfile>();
            UsersDisliked = new List<UserProfile>();
            UsersFavorite = new List<UserProfile>();
        }
    }
}
