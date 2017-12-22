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
    }
}