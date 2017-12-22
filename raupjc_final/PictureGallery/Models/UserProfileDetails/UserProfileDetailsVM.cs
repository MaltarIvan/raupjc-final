using PictureGallery.Core;
using PictureGallery.Models.Main;
using PictureGallery.Models.ManageProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.UserProfileDetails
{
    public class UserProfileDetailsVM
    {
        public UserProfileVM UserProfileVM { get; set; }
        public List<AlbumVM> Albums { get; set; }

        public UserProfileDetailsVM(UserProfile userProfile, List<Album> albums)
        {
            UserProfileVM = new UserProfileVM()
            {
                Id = userProfile.Id,
                UserName = userProfile.UserName,
                DateCreated = userProfile.DateCreated,
                ProfilePicture = new ProfilePictureVM(userProfile.ProfilePicture.Id, userProfile.Id, userProfile.ProfilePicture.Data)
            };
            Albums = new List<AlbumVM>();
            foreach (var album in albums)
            {
                Albums.Add(new AlbumVM(album));
            }
        }
    }
}
