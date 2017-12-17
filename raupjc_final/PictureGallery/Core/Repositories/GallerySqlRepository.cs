using PictureGallery.Core.Database;
using PictureGallery.CustomeExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
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

        public Task<List<UserProfile>> GetAllUsersAsync()
        {
            return _context.UserProfiles.Include(u => u.Favorites).Include(u => u.Albums).ToListAsync();
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
    }
}
