using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureGallery.Models.Main
{
    public class PictureVM
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Data { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
    }
}
