using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Main
{
    public class PictureVM
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Data { get; set; }
        public string DateCreated { get; set; }
        public string Description { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfDislikes { get; set; }

        public PictureVM(Guid id, Guid userId, string data, DateTime dateCreated, string description, int numberOfLikes, int numberOfDislikes)
        {
            Id = id;
            UserId = userId;
            Data = data;
            DateCreated = dateCreated.ToShortDateString();
            Description = description;
            NumberOfLikes = numberOfLikes;
            NumberOfDislikes = numberOfDislikes;
        }
    }
}
