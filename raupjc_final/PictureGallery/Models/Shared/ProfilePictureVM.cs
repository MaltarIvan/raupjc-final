using PictureGallery.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Shared
{
    public class ProfilePictureVM
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Url { get; set; }

        public ProfilePictureVM(Picture profilePicture)
        {
            Id = profilePicture.Id;
            UserId = profilePicture.UserId;
            Url = profilePicture.Url;
        }
    }
}
