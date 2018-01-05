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
        public UserProfileVM UserProfile { get; set; }
        public List<AlbumVM> Albums { get; set; }

        public ManageProfileVM(UserProfileVM userProfile, List<AlbumVM> albums)
        {
            UserProfile = userProfile;
            Albums = albums;
        }
    }
}
