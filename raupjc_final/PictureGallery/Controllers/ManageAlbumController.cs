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
using PictureGallery.Models.Shared;
using PictureGallery.CustomeExceptions;

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

        [HttpGet("ManageAlbum/Index/{id}")]
        public async Task<IActionResult> Index(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            if (currentUser == null)
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }

            Album album = await _repository.GetAlbumAsync(id);
            if (album == null)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }

            if (currentUser.Id != album.UserId)
            {
                return RedirectToAction("AlbumDetails", "UserProfileDetails", new { id = id });
            }

            List<PictureVM> picturesVM = new List<PictureVM>();
            foreach (var picture in album.Pictures)
            {
                string data = Convert.ToBase64String(picture.Data);
                picturesVM.Add(new PictureVM(picture));
            }

            AlbumVM albumVM = new AlbumVM(album.Id, currentUser.Id, album.DateCreated, album.Description);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser);
            ManageAlbumVM manageAlbumVM = new ManageAlbumVM(userProfileVM, albumVM, picturesVM);
            return View(manageAlbumVM);
        }

        public async Task<IActionResult> AddNewPicture(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid currentUserId = new Guid(applicationUser.Id);

            if (!await _repository.ContainsUserAsync(currentUserId))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }

            Album album = await _repository.GetAlbumAsync(id);

            if (album == null)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            else if (album.UserId != currentUserId)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
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
                    ModelState.AddModelError("CustomError", "Please choose either a GIF, JPG or PNG image.");
                    model.Picture = null;
                    return View(model);
                }
                else
                {
                    ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                    Guid currentUserId = new Guid(applicationUser.Id);
                    if (!await _repository.ContainsUserAsync(currentUserId))
                    {
                        return RedirectToAction("MakeNewProfile", "Main");
                    }
                    BinaryReader reader = new BinaryReader(model.Picture.OpenReadStream());
                    byte[] data = reader.ReadBytes((int)model.Picture.Length);
                    Album album = await _repository.GetAlbumAsync(model.AlbumId);
                    Picture picture = new Picture(currentUserId, model.Description, album, data);
                    await _repository.AddPictureAsync(picture, currentUserId);
                    return RedirectToAction("Index", album.Id);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ChangeAlbumDescription(Guid id, string description)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid currentUserId = new Guid(applicationUser.Id);
            if (!await _repository.ContainsUserAsync(currentUserId))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            else if (!await _repository.ContainsAlbumAsync(id))
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
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
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                Guid currentUserId = new Guid(applicationUser.Id);
                if (!await _repository.ContainsUserAsync(currentUserId))
                {
                    return RedirectToAction("MakeNewProfile", "Main");
                }
                Album album = await _repository.GetAlbumAsync(model.Id);
                album.Description = model.Description;
                try
                {
                    await _repository.UpdateAlbumAsync(album, currentUserId);
                }
                catch (UnauthorizedAttemptException)
                {
                    return View("~/Views/Shared/InvalidAttempt.cshtml");
                }
                return RedirectToAction("Index", new { id = model.Id });
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteAlbum(Guid albumId)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid currentUserId = new Guid(applicationUser.Id);
            if (!await _repository.ContainsUserAsync(currentUserId))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            Album album = await _repository.GetAlbumAsync(albumId);
            if (album == null)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            try
            {
                await _repository.DeleteAlbumAsync(album, currentUserId);
            }
            catch (UnauthorizedAttemptException)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            return RedirectToAction("Index", "ManageProfile");
        }
    }
}