using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Main
{
    public class UserProfileVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public ProfilePictureVM ProfilePicture { get; set; }

        public UserProfileVM(Guid id, string userName, DateTime dateCreated, ProfilePictureVM profilePicture)
        {
            Id = id;
            UserName = userName;
            DateCreated = dateCreated;
            ProfilePicture = profilePicture;
        }

        public UserProfileVM(Guid id, string userName, DateTime dateCreated)
        {
            Id = id;
            UserName = userName;
            DateCreated = dateCreated;
        }

        public UserProfileVM()
        {

        }
    }
}
