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
using PictureGallery.CustomeExceptions;

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
            Guid currentUserId = new Guid(applicationUser.Id);
            if (!await _repository.ContainsUserAsync(currentUserId))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }

            List<string> roles = (List<string>)await _userManager.GetRolesAsync(applicationUser);
            bool isAdmin = roles.Contains("Admin");
            
            Picture picture = await _repository.GetPictureAsync(id);
            if (picture == null)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            picture.User = await _repository.GetUserByIdAsync(picture.UserId);
            PictureDetailsVM pictureDetailsVM = new PictureDetailsVM(picture.UsersFavorite.Any(u => u.Id == currentUserId), currentUserId, isAdmin, picture);
            return View(pictureDetailsVM);
        }

        public async Task<IActionResult> LikePicture(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid currentUserId = new Guid(applicationUser.Id);
            if (!await _repository.ContainsUserAsync(currentUserId))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }

            await _repository.LikePictureAsync(id, currentUserId);
            return RedirectToAction("Index", new { id = id });
        }

        public async Task<IActionResult> DislikePicture(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid currentUserId = new Guid(applicationUser.Id);
            if (!await _repository.ContainsUserAsync(currentUserId))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            await _repository.DislikePictureAsync(id, currentUserId);
            return RedirectToAction("Index", new { id = id });
        }

        public async Task<IActionResult> AddNewComment(Guid pictureId)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            if (!await _repository.ContainsUserAsync(new Guid(applicationUser.Id)))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            if (!await _repository.ContainsPictureAsync(pictureId))
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
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
                if (user == null)
                {
                    return RedirectToAction("MakeNewProfile", "Main");
                }
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
            if (user == null)
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            Picture picture = await _repository.GetPictureAsync(pictureId);
            if (picture == null)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
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
            if (user == null)
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            Picture picture = await _repository.GetPictureAsync(pictureId);
            if (picture == null)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            if (user.Favorites.Contains(picture))
            {
                user.Favorites.Remove(picture);
                await _repository.UpdateUserAsync(user);
            }
            return RedirectToAction("Index", new { id = pictureId });
        }

        public async Task<IActionResult> DeletePicture(Guid pictureId)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid currentUserId = new Guid(applicationUser.Id);
            if (!await _repository.ContainsUserAsync(currentUserId))
            {
                return RedirectToAction("Index", "Main");
            }
            Picture picture = await _repository.GetPictureAsync(pictureId);
            if (picture == null)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            Guid albumId = picture.Album.Id;
            try
            {
                await _repository.DeletePictureAsync(picture, currentUserId);
            }
            catch (UnauthorizedAttemptException uae)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            return RedirectToAction("Index", "ManageAlbum", new { id = albumId });
        }

        public async Task<IActionResult> ChangePictureDescription(Guid pictureId, string description)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            if (!await _repository.ContainsUserAsync(new Guid(applicationUser.Id)))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            if (!await _repository.ContainsPictureAsync(pictureId))
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
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
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                Guid currentUserId = new Guid(applicationUser.Id);
                if (!await _repository.ContainsUserAsync(currentUserId))
                {
                    return RedirectToAction("MakeNewProfile", "Main");
                }
                Picture picture = await _repository.GetPictureAsync(model.Id);
                picture.Description = model.Description;
                try
                {
                    await _repository.UpdatePictureAsync(picture, currentUserId);
                }
                catch (UnauthorizedAttemptException uae)
                {
                    return View("~/Views/Shared/InvalidAttempt.cshtml");
                }
                return RedirectToAction("Index", new { id = model.Id });
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid currentUserId = new Guid(applicationUser.Id);
            if (!await _repository.ContainsUserAsync(currentUserId))
            {
                return RedirectToAction("MakeNewProfile", "Main");
            }
            Comment comment = await _repository.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            Guid currentPictureId = comment.Picture.Id;
            try
            {
                await _repository.DeleteCommentAsync(comment, currentUserId);
            }
            catch (UnauthorizedAttemptException uae)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            return RedirectToAction("Index", new { id = currentPictureId});
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HotPictureManage(Guid pictureId)
        {
            Picture picture = await _repository.GetPictureAsync(pictureId);
            if (picture == null)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            picture.IsHot = !picture.IsHot;
            try
            {
                await _repository.UpdatePictureAsync(picture, picture.UserId);
            }
            catch (UnauthorizedAttemptException uae)
            {
                return View("~/Views/Shared/InvalidAttempt.cshtml");
            }
            return RedirectToAction("Index", new { id = pictureId });
        }
    }
}