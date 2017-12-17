using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Core.Repositories
{
    public interface IGalleryRepository
    {
        Task<UserProfile> AddUserAsync(UserProfile user);
        Task<UserProfile> GetUserByIdAsync(Guid id);
        Task<UserProfile> UpdateUserAsync(UserProfile user);
        Task<List<UserProfile>> GetAllUsersAsync();
        Task<Picture> UpdatePictureAsync(Picture profilePicture);
    }
}
