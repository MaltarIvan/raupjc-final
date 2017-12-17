using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Core.Database
{
    public class GalleryDbContext : DbContext
    {
        public GalleryDbContext(string connectionString) : base(connectionString)
        {
        }

        public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<Album> Albums { get; set; }
        public IDbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserProfile>().HasKey(u => u.Id);
            modelBuilder.Entity<UserProfile>().HasMany(u => u.Albums).WithRequired(a => a.User);
            modelBuilder.Entity<UserProfile>().Property(u => u.UserName).IsOptional();
            modelBuilder.Entity<UserProfile>().HasMany(u => u.Favorites).WithMany(p => p.UsersFavorite);
            modelBuilder.Entity<UserProfile>().Property(u => u.DateCreated).IsRequired();
            modelBuilder.Entity<UserProfile>().HasOptional(u => u.ProfilePicture).WithRequired(p => p.User);

            modelBuilder.Entity<Album>().HasKey(a => a.Id);
            modelBuilder.Entity<Album>().HasRequired(a => a.User).WithMany(u => u.Albums);
            modelBuilder.Entity<Album>().Property(a => a.UserId).IsRequired();
            modelBuilder.Entity<Album>().Property(a => a.DateCreated).IsRequired();
            modelBuilder.Entity<Album>().Property(a => a.Description).IsRequired();
            modelBuilder.Entity<Album>().HasMany(a => a.Pictures).WithOptional(p => p.Album);

            modelBuilder.Entity<Picture>().HasKey(p => p.Id);
            modelBuilder.Entity<Picture>().Property(p => p.UserId).IsRequired();
            modelBuilder.Entity<Picture>().Property(p => p.Description).IsOptional();
            modelBuilder.Entity<Picture>().Property(p => p.DateCreted).IsRequired();
            modelBuilder.Entity<Picture>().HasOptional(p => p.Album).WithMany(a => a.Pictures);
            modelBuilder.Entity<Picture>().Property(p => p.Data).IsRequired();
            //modelBuilder.Entity<Picture>().HasRequired(p => p.User).WithOptional(u => u.ProfilePicture);
        }
    }
}
