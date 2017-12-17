using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Main
{
    public class ProfilePictureVM
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Data { get; set; }

        public ProfilePictureVM(Guid id, Guid userId, string data)
        {
            Id = id;
            UserId = userId;
            Data = data;
        }

        public ProfilePictureVM(Guid id, Guid userId, byte[] data)
        {
            Id = id;
            UserId = userId;
            Data = Convert.ToBase64String(data);
        }
    }
}
