using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PictureGallery.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using PictureGallery.Data;
using Microsoft.AspNetCore.Hosting;
using PictureGallery.Core;
using PictureGallery.Models.Main;
using PictureGallery.Models.UserProfileDetails;
using PictureGallery.Models.ManageProfile;

namespace PictureGallery.Controllers
{
    [Authorize]
    public class UserProfileDetailsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGalleryRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserProfileDetailsController(IGalleryRepository repository, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("Profile/{id}")]
        public async Task<IActionResult> Index(Guid id)
        {
            UserProfile userProfile = await _repository.GetUserByIdAsync(id);
            UserProfileDetailsVM userProfileDetailsVM = new UserProfileDetailsVM(userProfile, userProfile.Albums);
            return View(userProfileDetailsVM);
        }

        [HttpGet("Album/{id}")]
        public async Task<IActionResult> AlbumDetails(Guid id)
        {
            Album album = await _repository.GetAlbumAsync(id);
            AlbumVM albumVM = new AlbumVM(album);
            List<Picture> pictures = await _repository.GetPicturesFromAlbumAsync(id);
            List<PictureVM> picturesVM = new List<PictureVM>();
            foreach (var picture in pictures)
            {
                picturesVM.Add(new PictureVM(picture));
            }
            AlbumDetailsVM albumDetailsVM = new AlbumDetailsVM(album.UserId, albumVM, picturesVM);
            return View(albumDetailsVM);
        }


        //TODO staviti model UserProfileVM i u RedirectAction metodu poslati parametar UserId
        public async Task<IActionResult> FollowUser(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            UserProfile user = await _repository.GetUserByIdAsync(id);
            if (!currentUser.Following.Contains(user))
            {
                currentUser.Following.Add(user);
                await _repository.UpdateUserAsync(currentUser);
            }
            return RedirectToAction("Index", new { id = id });
        }
    }
}