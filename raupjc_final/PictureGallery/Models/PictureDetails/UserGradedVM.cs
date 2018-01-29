using PictureGallery.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.PictureDetails
{
    public class UserGradedVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        
        public UserGradedVM(UserProfile user)
        {
            Id = user.Id;
            UserName = user.UserName;
        }
    }
}
