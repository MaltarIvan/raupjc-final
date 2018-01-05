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
using PictureGallery.Models.PictureDetails;

namespace PictureGallery.Controllers
{
    [Authorize]
    public class PictureDetailsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGalleryRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PictureDetailsController(IGalleryRepository repository, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("Picture/Index/{id}")]
        public async Task<IActionResult> Index(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            List<string> roles = (List<string>)await _userManager.GetRolesAsync(applicationUser);
            bool isAdmin = roles.Contains("Admin");
            Guid currentUserId = new Guid(applicationUser.Id);
            Picture picture = await _repository.GetPictureAsync(id);
            PictureDetailsVM pictureDetailsVM = new PictureDetailsVM(picture.UsersFavorite.Any(u => u.Id == currentUserId), currentUserId == picture.UserId, isAdmin, picture);
            return View(pictureDetailsVM);
        }

        public async Task<IActionResult> LikePicture(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.LikePictureAsync(id, new Guid(applicationUser.Id));
            return RedirectToAction("Index", new { id = id });
        }

        public async Task<IActionResult> DislikePicture(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.DislikePictureAsync(id, new Guid(applicationUser.Id));
            return RedirectToAction("Index", new { id = id });
        }

        public IActionResult AddNewComment(Guid pictureId)
        {
            AddNewCommentVM addNewCommentVM = new AddNewCommentVM
            {
                PictureId = pictureId
            };
            return View(addNewCommentVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewComment(AddNewCommentVM model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                UserProfile user = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
                Picture picture = await _repository.GetPictureAsync(model.PictureId);
                Comment comment = new Comment
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    Text = model.Text,
                    DateCreated = DateTime.Now,
                    Picture = picture
                };
                await _repository.AddCommentAsync(comment);
                return RedirectToAction("Index", new { id = model.PictureId });
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> AddToFavorites(Guid pictureId)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile user = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            Picture picture = await _repository.GetPictureAsync(pictureId);
            if (!user.Favorites.Contains(picture))
            {
                user.Favorites.Add(picture);
                await _repository.UpdateUserAsync(user);
            }
            return RedirectToAction("Index", new { id = pictureId });
        }

        public async Task<IActionResult> RemoveFromFavorites(Guid pictureId)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile user = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            Picture picture = await _repository.GetPictureAsync(pictureId);
            if (user.Favorites.Contains(picture))
            {
                user.Favorites.Remove(picture);
                await _repository.UpdateUserAsync(user);
            }
            return RedirectToAction("Index", new { id = pictureId });
        }

        public async Task<IActionResult> DeletePicture(Guid pictureId)
        {
            Picture picture = await _repository.GetPictureAsync(pictureId);
            Guid albumId = picture.Album.Id;
            await _repository.DeletePictureAsync(picture);
            return RedirectToAction("Index", "ManageAlbum", new { id = albumId });
        }

        public IActionResult ChangePictureDescription(Guid pictureId, string description)
        {
            ChangePictureDescriptionVM changePictureDescriptionVM = new ChangePictureDescriptionVM
            {
                Id = pictureId
            };
            return View(changePictureDescriptionVM);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePictureDescription(ChangePictureDescriptionVM model)
        {
            if (ModelState.IsValid)
            {
                Picture picture = await _repository.GetPictureAsync(model.Id);
                picture.Description = model.Description;
                await _repository.UpdatePictureAsync(picture);
                return RedirectToAction("Index", new { id = model.Id });
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HotPictureManage(Guid pictureId, bool isHot)
        {
            Picture picture = await _repository.GetPictureAsync(pictureId);
            picture.IsHot = !isHot;
            await _repository.UpdatePictureAsync(picture);
            return RedirectToAction("Index", new { id = pictureId });
        }
    }
}