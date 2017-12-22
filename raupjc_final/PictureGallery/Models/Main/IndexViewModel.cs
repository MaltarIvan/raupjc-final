using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Main
{
    public class IndexViewModel
    {
        public string PictureGroup { get; set; }
        public UserProfileVM UserProfile { get; set; }
        public List<PictureVM> PicturesToPresent { get; set; }
        public List<UserProfileVM> UsersVM { get; set; }

        public IndexViewModel(string pictureGroup, UserProfileVM userProfile, List<PictureVM> picturesToPresent, List<UserProfileVM> usersVM)
        {
            PictureGroup = pictureGroup;
            UserProfile = userProfile;
            PicturesToPresent = picturesToPresent;
            UsersVM = usersVM;
        }
    }
}
