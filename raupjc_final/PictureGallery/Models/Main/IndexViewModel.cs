using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Main
{
    public class IndexViewModel
    {
        public UserProfileVM UserProfile { get; set; }
        public List<PictureVM> PicturesToPresent { get; set; }

        public IndexViewModel(UserProfileVM userProfile, List<PictureVM> picturesToPresent)
        {
            UserProfile = userProfile;
            PicturesToPresent = picturesToPresent;
        }
    }
}
