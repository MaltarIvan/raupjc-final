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
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using PictureGallery.Models.Main;
using PictureGallery.Models.ManageProfile;
using PictureGallery.Models.Shared;
using PictureGallery.CustomeExceptions;
using Microsoft.Extensions.Configuration;
using PictureGallery.Utilities;

namespace PictureGallery.Controllers
{
    [Authorize]
    public class ManageProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGalleryRepository _repository;
        private readonly string _storageAccountName;
        private readonly string _storageAccountKey;
        private readonly string _storageContainerName;

        public ManageProfileController(IGalleryRepository repository, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _repository = repository;
            _storageAccountName = configuration["StorageAccountSettings:StorageAccountName"];
            _storageAccountKey = configuration["StorageAccountSettings:StorageAccountKey1"];
            _storageContainerName = configuration["StorageAccountSettings:ResourceGroup"];
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            if (currentUser == null)
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture);
            List<Album> albums = await _repository.GetUsersAlbumsAsync(currentUser.Id);
            List<AlbumVM> albumsVM = new List<AlbumVM>();
            foreach (var album in albums)
            {
                albumsVM.Add(new AlbumVM(album.Id, album.UserId, album.DateCreated, album.Description));
            }
            ManageProfileVM manageProfileVM = new ManageProfileVM(currentUser, albumsVM);

            return View(manageProfileVM);
        }

        public async Task<ActionResult> ChangeProfilePicture()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            if (!await _repository.ContainsUserAsync(new Guid(applicationUser.Id)))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangeProfilePicture(ChangeProfilePictureVM model)
        {
            var validImageTypes = new string[]
            {
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
            };
            if (model.ProfilePictureUpload != null)
            {
                if (!validImageTypes.Contains(model.ProfilePictureUpload.ContentType))
                {
                    ModelState.AddModelError("CustomError", "Please choose either a GIF, JPG or PNG image.");
                    return View();
                }
            }
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
                if (currentUser == null)
                {
                    return RedirectToAction("MakeNewProfile", "Main");
                }
                Picture profilePicture = currentUser.ProfilePicture;

                byte[] data = null;
                BinaryReader reader = new BinaryReader(model.ProfilePictureUpload.OpenReadStream());
                data = reader.ReadBytes((int)model.ProfilePictureUpload.Length);

                var azureStorageUtility = new AzureStorageUtility(_storageAccountName, _storageAccountKey);
                Picture oldProfilepicture = profilePicture;
                profilePicture = await azureStorageUtility.Upload(_storageContainerName, data);
                profilePicture.UserId = currentUser.Id;
                try
                {
                    await _repository.DeletePictureAsync(oldProfilepicture, currentUser.Id);
                    currentUser.ProfilePicture = profilePicture;
                    await _repository.UpdateUserAsync(currentUser);
                    await azureStorageUtility.Delete(_storageContainerName, oldProfilepicture.Id);
                }
                catch (UnauthorizedAttemptException)
                {
                    return View("~/Views/Shared/InvalidAttempt.cshtml");
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> AddNewAlbum()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            if (!await _repository.ContainsUserAsync(new Guid(applicationUser.Id)))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddNewAlbum(AddAlbumVM model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                Guid currentUserId = new Guid(applicationUser.Id);
                if (!await _repository.ContainsUserAsync(currentUserId))
                {
                    return RedirectToAction("MakeNewProfile", "Main");
                }
                Album album = new Album(new Guid(applicationUser.Id), model.Description);
                UserProfile userProfile = await _repository.GetUserByIdAsync(currentUserId);
                userProfile.Albums.Add(album);
                await _repository.UpdateUserAsync(userProfile);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}