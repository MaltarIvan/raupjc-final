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
using PictureGallery.Models.ManageProfile;

namespace PictureGallery.Controllers
{
    [Authorize]
    public class ManageProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGalleryRepository _repository;

        public ManageProfileController(IGalleryRepository repository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));

            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture.Id, currentUser.Id, currentUser.ProfilePicture.Data);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser.Id, currentUser.UserName, currentUser.DateCreated, profilePictureVM);
            List<Album> albums = currentUser.Albums;
            List<AlbumVM> albumsVM = new List<AlbumVM>();
            foreach (var album in albums)
            {
                albumsVM.Add(new AlbumVM(album.Id, album.UserId, album.DateCreated, album.Description));
            }
            ManageProfileVM manageProfileVM = new ManageProfileVM(userProfileVM, albumsVM);

            return View(manageProfileVM);
        }

        public ActionResult ChangeProfilePicture()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangeProfilePicture(ChangeProfilePictureVM model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
                Picture profilePicture = currentUser.ProfilePicture;

                byte[] data = null;
                BinaryReader reader = new BinaryReader(model.ProfilePictureUpload.OpenReadStream());
                data = reader.ReadBytes((int)model.ProfilePictureUpload.Length);

                profilePicture.Data = data;
                await _repository.UpdatePictureAsync(profilePicture);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult AddNewAlbum()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddNewAlbum(AddAlbumVM model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                Album album = new Album(new Guid(applicationUser.Id), model.Description);
                UserProfile userProfile = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
                userProfile.Albums.Add(album);
                await _repository.UpdateUserAsync(userProfile);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}