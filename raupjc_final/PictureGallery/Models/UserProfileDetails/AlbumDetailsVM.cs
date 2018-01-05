using PictureGallery.Models.Main;
using PictureGallery.Models.ManageProfile;
using PictureGallery.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.UserProfileDetails
{
    public class AlbumDetailsVM
    {
        public Guid UserId { get; set; }
        public AlbumVM AlbumVM { get; set; }
        public List<PictureVM> PicturesVM { get; set; }

        public AlbumDetailsVM(Guid userId, AlbumVM albumVM, List<PictureVM> picturesVM)
        {
            UserId = userId;
            AlbumVM = albumVM;
            PicturesVM = picturesVM;
        }
    }
}
