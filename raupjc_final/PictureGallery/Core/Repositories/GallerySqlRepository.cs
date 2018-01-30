using PictureGallery.Core.Database;
using PictureGallery.CustomeExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
//using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using PictureGallery.Core.Repositories;
using System.Data.Entity.Validation;

namespace PictureGallery.Core
{
    public class GallerySqlRepository : IGalleryRepository
    {
        private GalleryDbContext _context;
        public GallerySqlRepository(GalleryDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile> AddUserAsync(UserProfile user)
        {
            if (_context.UserProfiles.Any(u => u.Id == user.Id ||u.UserName == user.UserName))
            {
                throw new DuplicateItemException();
            }
            else
            {
                _context.UserProfiles.Add(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public Task<UserProfile> GetUserByIdAsync(Guid id)
        {
            return _context.UserProfiles.Include(u => u.Favorites).Include(u => u.ProfilePicture).Include(u => u.Following).Include(u => u.Followers.Select(f => f.ProfilePicture)).SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> ContainsUserAsync(Guid id)
        {
            return await _context.UserProfiles.AnyAsync(u => u.Id == id);
        }

        public async Task<List<UserProfile>> GetUserProfilesAsync(Guid currentUserId)
        {
            return await _context.UserProfiles.Where(u => u.Id != currentUserId).Include(u => u.ProfilePicture).ToListAsync();
        }

        public async Task<UserProfile> UpdateUserAsync(UserProfile user)
        {
            /*
            _context.UserProfiles.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChangesAsync();
            */

            var userProfile = _context.UserProfiles.First(u => u.Id == user.Id);
            userProfile.UserName = user.UserName;
            userProfile.ProfilePicture = user.ProfilePicture;
            userProfile.DateCreated = user.DateCreated;
            userProfile.Albums = user.Albums;
            userProfile.Favorites = user.Favorites;
            userProfile.Following = user.Following;
            userProfile.Followers = user.Followers;
            await _context.SaveChangesAsync();
            return userProfile;
        }

        public async Task<List<UserProfile>> GetAllUsersAsync()
        {
            return await _context.UserProfiles.Include(u => u.Favorites).Include(u => u.Albums).ToListAsync();
        }

        public async Task<Album> GetAlbumAsync(Guid id)
        {
            return await _context.Albums.Include(a => a.Pictures.Select(p => p.Comments)).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Album> UpdateAlbumAsync(Album album, Guid userId)
        {
            if (album.UserId == userId)
            {
                _context.Albums.Attach(album);
                _context.Entry(album).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            } else
            {
                throw new UnauthorizedAttemptException();
            }
            return album;
        }

        public async Task<Album> DeleteAlbumAsync(Album album, Guid userId)
        {
            if (album.UserId == userId)
            {
                foreach (var picture in album.Pictures.ToList())
                {
                    _context.Pictures.Remove(picture);
                }
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UnauthorizedAttemptException();
            }
            return album;
        }

        public async Task<List<Album>> GetUsersAlbumsAsync(Guid userId)
        {
            return await _context.Albums.Where(a => a.UserId == userId).OrderByDescending(a => a.DateCreated).ToListAsync();
        }

        public async Task<bool> ContainsAlbumAsync(Guid id)
        {
            return await _context.Albums.AnyAsync(a => a.Id == id);
        }

        public async Task<Picture> AddPictureAsync(Picture picture, Guid userId)
        {
            if (picture.UserId == userId)
            {
                if (_context.Pictures.Any(p => p.Id == picture.Id))
                {
                    throw new DuplicateItemException();
                }
                else
                {
                    _context.Pictures.Add(picture);
                    await _context.SaveChangesAsync();
                }
            }
            return picture;
        }

        public async Task<Picture> GetPictureAsync(Guid id)
        {
            return await _context.Pictures.Include(p => p.Comments.Select(c => c.User.ProfilePicture)).Include(p => p.UsersFavorite).Include(p => p.Album).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Picture> GetPictureWithUsersGradedAsync(Guid pictureId)
        {
            return await _context.Pictures.Include(p => p.Comments.Select(c => c.User.ProfilePicture)).Include(p => p.UsersFavorite).Include(p => p.Album).Include(p => p.UsersLiked).Include(p => p.UsersDisliked).FirstOrDefaultAsync(p => p.Id == pictureId);
        }

        public async Task<List<Picture>> GetPicturesFromAlbumAsync(Guid id)
        {
            return await _context.Pictures.Where(p => p.Album.Id == id).OrderByDescending(p => p.DateCreted).Include(p => p.Comments).ToListAsync();
        }

        public async Task<List<Picture>> GetHotPicturesAsync()
        {
            return await _context.Pictures.Where(p => p.IsHot).Include(p => p.Comments).ToListAsync();
        }

        public async Task<Picture> UpdatePictureAsync(Picture picture, Guid userId)
        {
            if (picture.UserId == userId)
            {
                _context.Pictures.Attach(picture);
                _context.Entry(picture).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UnauthorizedAttemptException();
            }
            return picture;
        }

        public async Task<Picture> DeletePictureAsync(Picture picture, Guid userId)
        {
            if (picture.UserId == userId)
            {
                _context.Pictures.Remove(picture);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UnauthorizedAttemptException();
            }
            return picture;
        }

        public async Task<Picture> LikePictureAsync(Guid id, Guid userId)
        {
            Picture picture = await _context.Pictures.Include(p => p.UsersLiked).Include(p => p.UsersDisliked).FirstAsync(u => u.Id == id);
            if (!picture.UsersLiked.Any(u => u.Id == userId))
            {
                UserProfile userProfile = await _context.UserProfiles.FirstAsync(u => u.Id == userId);
                picture.NumberOfLikes++;
                userProfile.PicturesLiked.Add(picture);
                if (picture.UsersDisliked.Any(u => u.Id == userId))
                {
                    picture.NumberOfDislikes--;
                    userProfile.PicturesDisliked.Remove(picture);
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                UserProfile userProfile = await _context.UserProfiles.FirstAsync(u => u.Id == userId);
                picture.NumberOfLikes--;
                userProfile.PicturesLiked.Remove(picture);
                await _context.SaveChangesAsync();
            }
            return picture;
        }

        public async Task<Picture> DislikePictureAsync(Guid id, Guid userId)
        {
            Picture picture = await _context.Pictures.Include(p => p.UsersLiked).Include(p => p.UsersDisliked).FirstAsync(u => u.Id == id);
            if (!picture.UsersDisliked.Any(u => u.Id == userId))
            {
                UserProfile userProfile = await _context.UserProfiles.FirstAsync(u => u.Id == userId);
                picture.NumberOfDislikes++;
                userProfile.PicturesDisliked.Add(picture);
                if (picture.UsersLiked.Any(u => u.Id == userId))
                {
                    picture.NumberOfLikes--;
                    userProfile.PicturesLiked.Remove(picture);
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                UserProfile userProfile = await _context.UserProfiles.FirstAsync(u => u.Id == userId);
                picture.NumberOfDislikes--;
                userProfile.PicturesDisliked.Remove(picture);
                await _context.SaveChangesAsync();
            }
            return picture;
        }

        public async Task<List<Picture>> GetAllPicturesAsync()
        {
            return await _context.Pictures.Where(p => p.Album != null).OrderByDescending(p => p.DateCreted).Include(p => p.Comments).ToListAsync();
        }

        public async Task<bool> ContainsPictureAsync(Guid id)
        {
            return await _context.Pictures.AnyAsync(p => p.Id == id);
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> DeleteCommentAsync(Comment comment, Guid userId)
        {
            if (comment.User.Id == userId)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UnauthorizedAttemptException();
            }
            return comment;
        }

        public async Task<List<Picture>> GetAllPicturesFromUserAsync(Guid id)
        {
            return await _context.Pictures.Where(p => p.UserId == id && p.User == null).ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(Guid commentId)
        {
            return await _context.Comments.Include(c => c.User).Include(c => c.Picture).FirstOrDefaultAsync(c => c.Id == commentId);
        }
    }
}
