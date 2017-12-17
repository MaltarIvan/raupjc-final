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
        void UpdateUserAsync(UserProfile user);
        Task<List<UserProfile>> GetAllUsersAsync();
    }
}
