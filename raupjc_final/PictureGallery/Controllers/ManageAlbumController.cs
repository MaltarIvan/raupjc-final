using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PictureGallery.Data;
using Microsoft.AspNetCore.Identity;
using PictureGallery.Core;
using Microsoft.AspNetCore.Authorization;
using PictureGallery.Core.Repositories;
using PictureGallery.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using PictureGallery.Models.Main;
using Microsoft.AspNetCore.Hosting;
using PictureGallery.Models.ManageProfile;
using PictureGallery.Models.ManageAlbum;

namespace PictureGallery.Controllers
{
    [Authorize]
    public class ManageAlbumController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGalleryRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ManageAlbumController(IGalleryRepository repository, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("Index/{id}")]
        public async Task<IActionResult> Index(Guid id)
        {
            Album album = await _repository.GetAlbumAsync(id);

            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));

            List<PictureVM> picturesVM = new List<PictureVM>();
            foreach (var picture in album.Pictures)
            {
                string data = Convert.ToBase64String(picture.Data);
                picturesVM.Add(new PictureVM(picture.Id, currentUser.Id, data, picture.DateCreted, picture.Description, picture.NumberOfLikes, picture.NumberOfDislikes));
            }

            AlbumVM albumVM = new AlbumVM(album.Id, currentUser.Id, album.DateCreated, album.Description);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser.Id, currentUser.UserName, currentUser.DateCreated);
            ManageAlbumVM manageAlbumVM = new ManageAlbumVM(userProfileVM, albumVM, picturesVM);
            return View(manageAlbumVM);
        }

        public IActionResult AddNewPicture(Guid id)
        {
            AddNewPictureVM addNewPictureVM = new AddNewPictureVM
            {
                AlbumId = id
            };
            return View(addNewPictureVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPicture(AddNewPictureVM model)
        {
            var validImageTypes = new string[]
            {
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
            };
            if (ModelState.IsValid)
            {
                if (!validImageTypes.Contains(model.Picture.ContentType))
                {
                    return View();
                }
                else
                {
                    ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                    BinaryReader reader = new BinaryReader(model.Picture.OpenReadStream());
                    byte[] data = reader.ReadBytes((int)model.Picture.Length);
                    Album album = await _repository.GetAlbumAsync(model.AlbumId);
                    Picture picture = new Picture(new Guid(applicationUser.Id), model.Description, album, data);
                    await _repository.AddPictureAsync(picture);
                    return RedirectToAction("Index", album.Id);
                }
            }
            return View(model);
        }

        public IActionResult ChangeAlbumDescription(Guid id, string description)
        {
            ChangeAlbumDescriptionVM changeAlbumDescriptionVM = new ChangeAlbumDescriptionVM
            {
                Id = id,
                Description = description
            };
            return View(changeAlbumDescriptionVM);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAlbumDescription(ChangeAlbumDescriptionVM model)
        {
            if (ModelState.IsValid)
            {
                Album album = await _repository.GetAlbumAsync(model.Id);
                album.Description = model.Description;
                await _repository.UpdateAlbumAsync(album);
                return RedirectToAction("Index", new { id = model.Id });
            }
            return View(model);
        }
    }
}