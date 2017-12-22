using PictureGallery.Models.Main;
using PictureGallery.Models.ManageProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.ManageAlbum
{
    public class ManageAlbumVM
    {
        public UserProfileVM UserProfileVM;
        public AlbumVM AlbumVM;
        public List<PictureVM> PicturesVM;

        public ManageAlbumVM(UserProfileVM userProfileVM, AlbumVM albumVM, List<PictureVM> picturesVM)
        {
            UserProfileVM = userProfileVM;
            AlbumVM = albumVM;
            PicturesVM = picturesVM;
        }
    }
}
