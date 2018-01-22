using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PictureGallery.Core.Repositories;
using PictureGallery.Core.Database;

namespace PictureGallery.Core.Repositories
{
    public class MessengerSqlRepository :IMessengerRepository
    {
        private GalleryDbContext _context;
        public MessengerSqlRepository(GalleryDbContext context)
        {
            _context = context;
        }
    }
}
