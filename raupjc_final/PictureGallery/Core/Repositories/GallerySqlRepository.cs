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
            return _context.UserProfiles.Include(u => u.Favorites).Include(u => u.Albums).Include(u => u.ProfilePicture).SingleOrDefaultAsync(u => u.Id == id);
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
            await _context.SaveChangesAsync();
            return userProfile;
        }

        public async Task<List<UserProfile>> GetAllUsersAsync()
        {
            return await _context.UserProfiles.Include(u => u.Favorites).Include(u => u.Albums).ToListAsync();
        }

        public async Task<Album> GetAlbumAsync(Guid id)
        {
            return await _context.Albums.Include(a => a.Pictures).FirstAsync(a => a.Id == id);
        }

        public async Task<Album> UpdateAlbumAsync(Album album)
        {
            _context.Albums.Attach(album);
            _context.Entry(album).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return album;
        }

        public async Task<Picture> AddPictureAsync(Picture picture)
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
            return picture;
        }

        public async Task<Picture> GetPictureAsync(Guid id)
        {
            return await _context.Pictures.Include(p => p.Comments.Select(c => c.User.ProfilePicture)).FirstAsync(p => p.Id == id);
        }

        // nepotrebno ?
        public async Task<Picture> AddPictureToAlbumAsync(Guid id, Picture picture)
        {
            Album album = await _context.Albums.FirstAsync(a => a.Id == id);
            album.Pictures.Add(picture);
            await _context.SaveChangesAsync();
            return picture;
        }

        public async Task<List<Picture>> GetPicturesFromAlbum(Guid id)
        {
            return await _context.Pictures.Where(p => p.Album.Id == id).ToListAsync();
        }

        public async Task<Picture> UpdatePictureAsync(Picture picture)
        {
            _context.Pictures.Attach(picture);
            _context.Entry(picture).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return picture;

            /*
            var pic = _context.Pictures.First(p => p.Id == picture.Id);
            pic.Data = picture.Data;
            pic.Description = picture.Description;
            await _context.SaveChangesAsync();
            return pic;
            */
        }

        public async Task<Picture> LikePictureAsync(Guid id, Guid userId)
        {
            Picture picture = await _context.Pictures.Include(p => p.UsersLiked).FirstAsync(u => u.Id == id);
            if (!picture.UsersLiked.Any(u => u.Id == userId))
            {
                UserProfile userProfile = await _context.UserProfiles.FirstAsync(u => u.Id == userId);
                picture.NumberOfLikes++;
                userProfile.PicturesLiked.Add(picture);
                await _context.SaveChangesAsync();
            }
            return picture;
        }

        public async Task<Picture> DislikePictureAsync(Guid id, Guid userId)
        {
            Picture picture = await _context.Pictures.Include(p => p.UsersDisliked).FirstAsync(u => u.Id == id);
            if (!picture.UsersDisliked.Any(u => u.Id == userId))
            {
                UserProfile userProfile = await _context.UserProfiles.FirstAsync(u => u.Id == userId);
                picture.NumberOfDislikes++;
                userProfile.PicturesDisliked.Add(picture);
                await _context.SaveChangesAsync();
            }
            return picture;
        }

        public async Task<List<Picture>> GetAllPicturesAsync()
        {
            return await _context.Pictures.Where(p => p.Album != null).ToListAsync();
        }

        public async Task<List<UserProfile>> GetUserProfilesAsync(Guid currentUserId)
        {
            return await _context.UserProfiles.Where(u => u.Id != currentUserId).Include(u => u.ProfilePicture).ToListAsync();
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetComments(Guid pictureId)
        {
            return  await _context.Comments.Include(c => c.User.ProfilePicture).Where(c => c.Picture.Id == pictureId).ToListAsync();
        }
    }
}
