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
        Task<List<Picture>> GetHotPicturesAsync();
        Task<Picture> UpdatePictureAsync(Picture picture, Guid userId);
        Task<Picture> LikePictureAsync(Guid Id, Guid userId);
        Task<Picture> DislikePictureAsync(Guid id, Guid userId);
        Task<List<Picture>> GetPicturesFromAlbumAsync(Guid id);
        Task<Picture> AddPictureAsync(Picture picture, Guid userId);
        Task<Picture> DeletePictureAsync(Picture picture, Guid userId);

        Task<Album> GetAlbumAsync(Guid id);
        Task<Album> UpdateAlbumAsync(Album album, Guid userId);
        Task<Album> DeleteAlbumAsync(Album album, Guid userId);
        
        Task<Comment> AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsAsync(Guid commentId);
        Task<Comment> GetCommentByIdAsync(Guid commentId);
        Task<Comment> DeleteCommentAsync(Comment comment, Guid userId);
    }
}
