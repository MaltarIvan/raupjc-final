using PictureGallery.Models.Main;
using PictureGallery.Models.ManageProfile;
using PictureGallery.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.ManageAlbum
{
    public class ManageAlbumVM
    {
        public UserProfileVM UserProfileVM { get; set; }
        public AlbumVM AlbumVM;
        public List<PictureVM> PicturesVM { get; set; }
        public bool IsUsersAlbum { get; set; }

        public ManageAlbumVM(UserProfileVM userProfileVM, AlbumVM albumVM, List<PictureVM> picturesVM)
        {
            IsUsersAlbum = userProfileVM.Id == albumVM.UserId;
            UserProfileVM = userProfileVM;
            AlbumVM = albumVM;
            PicturesVM = picturesVM;
        }
    }
}
