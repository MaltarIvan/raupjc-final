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

        [HttpGet("Picture/{id}")]
        public async Task<IActionResult> Index(Guid id)
        {
            Picture picture = await _repository.GetPictureAsync(id);
            PictureDetailsVM pictureDetailsVM = new PictureDetailsVM(picture);
            return View(pictureDetailsVM);
        }

        public async Task<IActionResult> LikePicture(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.LikePictureAsync(id, new Guid(applicationUser.Id));
            return RedirectToAction("Index", id);
        }

        public async Task<IActionResult> DislikePicture(Guid id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.DislikePictureAsync(id, new Guid(applicationUser.Id));
            return RedirectToAction("Index", id);
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
                await _repository.AddComment(comment);
                return RedirectToAction("Index", model.PictureId);
            }
            else
            {
                return View(model);
            }
        }
    }
}