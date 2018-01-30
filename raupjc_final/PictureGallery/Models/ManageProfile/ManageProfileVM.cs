using PictureGallery.Core;
using PictureGallery.Models.Main;
using PictureGallery.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.ManageProfile
{
    public class ManageProfileVM
    {
        public UserProfileVM UserProfileVM { get; set; }
        public List<AlbumVM> Albums { get; set; }
        public List<UserProfileVM> Followers { get; set; }

        public ManageProfileVM(UserProfile currentUserProfile, List<AlbumVM> albums)
        {
            UserProfileVM = new UserProfileVM(currentUserProfile);
            Albums = albums;
            Followers = new List<UserProfileVM>();
            foreach (var follower in currentUserProfile.Followers)
            {
                Followers.Add(new UserProfileVM(follower));
            }
        }
    }
}
