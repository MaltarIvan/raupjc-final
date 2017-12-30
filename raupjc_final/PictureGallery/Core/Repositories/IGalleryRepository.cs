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
        Task<List<UserProfile>> GetUserProfilesAsync(Guid currentUserId);
        Task<List<UserProfile>> GetFollowingUserProfiles(Guid currentUserId);

        Task<Picture> GetPictureAsync(Guid id);
        Task<List<Picture>> GetAllPicturesAsync();
        Task<List<Picture>> GetAllPicturesFromUserAsync(Guid id);
        Task<List<Picture>> GetUsersFavoritePicturesAsync(UserProfile user);
        Task<List<Picture>> GetHotPicturesAsync();
        Task<Picture> UpdatePictureAsync(Picture picture);
        Task<Picture> LikePictureAsync(Guid Id, Guid userId);
        Task<Picture> DislikePictureAsync(Guid id, Guid userId);
        Task<List<Picture>> GetPicturesFromAlbumAsync(Guid id);
        Task<Picture> AddPictureAsync(Picture picture);
        Task<Picture> AddPictureToAlbumAsync(Guid Id, Picture picture);
        Task<Picture> DeletePictureAsync(Picture picture);

        Task<Album> GetAlbumAsync(Guid id);
        Task<Album> UpdateAlbumAsync(Album album);
        Task<Album> DeleteAlbumAsync(Album album);
        
        Task<Comment> AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsAsync(Guid pictureId);
    }
}
