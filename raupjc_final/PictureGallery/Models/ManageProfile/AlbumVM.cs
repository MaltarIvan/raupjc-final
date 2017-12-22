using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.ManageProfile
{
    public class AlbumVM
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public String DateCreated { get; set; }
        public string Description { get; set; }

        public AlbumVM(Guid id, Guid userId, DateTime dateCreated, string description)
        {
            Id = id;
            UserId = userId;
            DateCreated = dateCreated.ToShortDateString();
            Description = description;
        }
    }
}
