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
        Task<Picture> GetPictureAsync(Guid id);
        Task<Picture> UpdatePictureAsync(Picture profilePicture);
        Task<Picture> LikePictureAsync(Guid Id, Guid userId);
        Task<Picture> DislikePictureAsync(Guid id, Guid userId);
        Task<List<Picture>> GetPicturesFromAlbum(Guid id);
        Task<Picture> AddPictureAsync(Picture picture);
        Task<Picture> AddPictureToAlbumAsync(Guid Id, Picture picture);
        Task<Album> GetAlbumAsync(Guid id);
        Task<Album> UpdateAlbumAsync(Album album);
        Task<List<Picture>> GetAllPicturesAsync();
        Task<List<UserProfile>> GetUserProfilesAsync(Guid currentUserId);
        Task<Comment> AddComment(Comment comment);
        Task<List<Comment>> GetComments(Guid pictureId);
    }
}
