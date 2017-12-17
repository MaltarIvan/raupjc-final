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

namespace PictureGallery.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGalleryRepository _repository;

        public MainController(IGalleryRepository repository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public object GallerySqlRepository { get; private set; }
        public Picture Picture { get; private set; }

        public async Task<IActionResult> Index()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            if (currentUser == null)
            {
                /*
                currentUser = CreateNewUserProfile(new Guid(applicationUser.Id));
                await _repository.AddUserAsync(currentUser);
                */
                return RedirectToAction("MakeNewProfile");
            }
            return View();
        }

        public ActionResult MakeNewProfile()
        {
            return View(new AddUserProfileVM());
        }

        [HttpPost]
        public async Task<ActionResult> MakeNewProfile(AddUserProfileVM model)
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
                    //ModelState.AddModelError("CustomError", "Please choose either a GIF, JPG or PNG image.");
                    return View();
                }
            }
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                Picture profilePicture = null;

                if (model.ProfilePictureUpload != null && model.ProfilePictureUpload.Length > 0)
                {
                    byte[] data = null;
                    BinaryReader reader = new BinaryReader(model.ProfilePictureUpload.OpenReadStream());
                    data = reader.ReadBytes((int)model.ProfilePictureUpload.Length);
                    profilePicture = new Picture(new Guid(applicationUser.Id), data);
                }

                UserProfile currentUser = new UserProfile(new Guid(applicationUser.Id));

                if (profilePicture != null)
                {
                    profilePicture.User = currentUser;
                    currentUser.ProfilePicture = profilePicture;
                }

                currentUser.UserName = model.UserName;
                //_repository.UpdateUserAsync(currentUser);
                await _repository.AddUserAsync(currentUser);
                
                return RedirectToAction("Index");
            }
            return View();
        }

        private UserProfile CreateNewUserProfile(Guid id)
        {
            return new UserProfile(id);
        }
    }
}