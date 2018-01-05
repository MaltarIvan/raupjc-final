using PictureGallery.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Shared
{
    public class UserProfileVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public ProfilePictureVM ProfilePicture { get; set; }

        public UserProfileVM(UserProfile userProfile)
        {
            Id = userProfile.Id;
            UserName = userProfile.UserName;
            DateCreated = userProfile.DateCreated;
            ProfilePicture = new ProfilePictureVM(userProfile.ProfilePicture);
        }
    }
}
