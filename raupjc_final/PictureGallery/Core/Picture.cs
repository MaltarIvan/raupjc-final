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
        
        public Picture(Guid userId, string description, Album album, byte[] data)
        {
            Id = new Guid();
            UserId = userId;
            Description = description;
            DateCreted = DateTime.Now;
            Album = album;
            Data = data;
            UsersFavorite = new List<UserProfile>();
        }

        public Picture(Guid userId, byte[] data)
        {
            Id = new Guid();
            UserId = userId;
            Data = data;
            DateCreted = DateTime.Now;
        }

        public Picture()
        {
        }
    }
}
