using PictureGallery.Core;
using PictureGallery.Models.Main;
using PictureGallery.Models.ManageProfile;
using PictureGallery.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.UserProfileDetails
{
    public class UserProfileDetailsVM
    {
        public bool IsFollowing;
        public UserProfileVM UserProfileVM { get; set; }
        public List<AlbumVM> Albums { get; set; }

        public UserProfileDetailsVM(bool isFollowing, UserProfile userProfile)
        {
            IsFollowing = isFollowing;
            UserProfileVM = new UserProfileVM(userProfile);
            Albums = new List<AlbumVM>();
            foreach (var album in userProfile.Albums)
            {
                Albums.Add(new AlbumVM(album));
            }
        }
    }
}
