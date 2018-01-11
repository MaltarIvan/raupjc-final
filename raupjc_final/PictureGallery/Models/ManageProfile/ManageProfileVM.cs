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

        public ManageProfileVM(UserProfileVM userProfile, List<AlbumVM> albums)
        {
            UserProfileVM = userProfile;
            Albums = albums;
        }
    }
}
