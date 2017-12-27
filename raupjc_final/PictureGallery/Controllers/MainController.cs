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

namespace PictureGallery.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGalleryRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public MainController(IGalleryRepository repository, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        public object GallerySqlRepository { get; private set; }
        public Picture Picture { get; private set; }

        public async Task<IActionResult> Index()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            if (currentUser == null)
            {
                currentUser = CreateNewUserProfile(new Guid(applicationUser.Id));
                await _repository.AddUserAsync(currentUser);
                return RedirectToAction("MakeNewProfile");
            }
            return RedirectToAction("All");
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
                byte[] data = null;

                if (model.ProfilePictureUpload != null && model.ProfilePictureUpload.Length > 0)
                {
                    BinaryReader reader = new BinaryReader(model.ProfilePictureUpload.OpenReadStream());
                    data = reader.ReadBytes((int)model.ProfilePictureUpload.Length);
                    profilePicture = new Picture(new Guid(applicationUser.Id), data);
                }
                else
                {
                    var webRoot = _hostingEnvironment.WebRootPath;
                    var file = Path.Combine(webRoot, "Content\\default-profile-picture.png");
                    data = System.IO.File.ReadAllBytes(file);
                    profilePicture = new Picture(new Guid(applicationUser.Id), data);
                }

                UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));

                if (profilePicture != null)
                {
                    profilePicture.User = currentUser;
                    currentUser.ProfilePicture = profilePicture;
                }

                currentUser.UserName = model.UserName;
                await _repository.UpdateUserAsync(currentUser);
                
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> All()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            List<Picture> pictures = await _repository.GetAllPicturesAsync();

            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture.Id, currentUser.Id, currentUser.ProfilePicture.Data);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser.Id, currentUser.UserName, currentUser.DateCreated, profilePictureVM);
            List<PictureVM> picturesToPresent = new List<PictureVM>();
            foreach (var item in pictures)
            {
                string data = Convert.ToBase64String(item.Data);
                picturesToPresent.Add(new PictureVM(item.Id, item.UserId, data, item.DateCreted, item.Description, item.NumberOfLikes, item.NumberOfDislikes));
            }

            // get all users
            List<UserProfileVM> userProfilesVM = await GetAllUsersVMAsync(currentUser.Id);

            //get following users
            List<UserProfileVM> followingUserProfilesVM = GetFollowingUsersVM(currentUser);

            IndexViewModel indexViewModel = new IndexViewModel("Newest", userProfileVM, picturesToPresent, userProfilesVM, followingUserProfilesVM);

            return View("Index", indexViewModel);
        }

        public async Task<IActionResult> Favorites()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            List<Picture> pictures = currentUser.Favorites;

            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture.Id, currentUser.Id, currentUser.ProfilePicture.Data);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser.Id, currentUser.UserName, currentUser.DateCreated, profilePictureVM);
            List<PictureVM> picturesToPresent = new List<PictureVM>();
            foreach (var item in pictures)
            {
                string data = Convert.ToBase64String(item.Data);
                picturesToPresent.Add(new PictureVM(item.Id, item.UserId, data, item.DateCreted, item.Description, item.NumberOfLikes, item.NumberOfDislikes));
            }

            // get all users
            List<UserProfileVM> userProfilesVM = await GetAllUsersVMAsync(currentUser.Id);

            //get following users
            List<UserProfileVM> followingUserProfilesVM = GetFollowingUsersVM(currentUser);

            IndexViewModel indexViewModel = new IndexViewModel("Your favorites", userProfileVM, picturesToPresent, userProfilesVM, followingUserProfilesVM);
            
            return View("Index", indexViewModel);
        }

        public async Task<IActionResult> FollowingPictures()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            List<Picture> pictures = new List<Picture>();
            foreach (var user in currentUser.Following)
            {
                pictures.AddRange(await _repository.GetAllPicturesFromUserAsync(user.Id));
            }

            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture.Id, currentUser.Id, currentUser.ProfilePicture.Data);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser.Id, currentUser.UserName, currentUser.DateCreated, profilePictureVM);
            List<PictureVM> picturesToPresent = new List<PictureVM>();
            foreach (var item in pictures)
            {
                string data = Convert.ToBase64String(item.Data);
                picturesToPresent.Add(new PictureVM(item.Id, item.UserId, data, item.DateCreted, item.Description, item.NumberOfLikes, item.NumberOfDislikes));
            }

            // get all users
            List<UserProfileVM> userProfilesVM = await GetAllUsersVMAsync(currentUser.Id);

            //get following users
            List<UserProfileVM> followingUserProfilesVM = GetFollowingUsersVM(currentUser);

            IndexViewModel indexViewModel = new IndexViewModel("Pictures You Follow", userProfileVM, picturesToPresent, userProfilesVM, followingUserProfilesVM);

            return View("Index", indexViewModel);
        }

        public async Task<List<UserProfileVM>> GetAllUsersVMAsync(Guid id)
        {
            List<UserProfile> userProfiles = await _repository.GetUserProfilesAsync(id);
            List<UserProfileVM> userProfilesVM = new List<UserProfileVM>();
            foreach (var userProfile in userProfiles)
            {
                ProfilePictureVM usersProfilePictureVM = new ProfilePictureVM(userProfile.Id, userProfile.ProfilePicture.Id, Convert.ToBase64String(userProfile.ProfilePicture.Data));
                userProfilesVM.Add(new UserProfileVM(userProfile.Id, userProfile.UserName, userProfile.DateCreated, usersProfilePictureVM));
            }
            return userProfilesVM;
        }

        public List<UserProfileVM> GetFollowingUsersVM(UserProfile currentUser)
        {
            List<UserProfile> followingUserProfiles = currentUser.Following;
            List<UserProfileVM> followingUserProfilesVM = new List<UserProfileVM>();
            foreach (var followingUserProfile in followingUserProfiles)
            {
                ProfilePictureVM usersProfilePictureVM = new ProfilePictureVM(followingUserProfile.Id, followingUserProfile.ProfilePicture.Id, Convert.ToBase64String(followingUserProfile.ProfilePicture.Data));
                followingUserProfilesVM.Add(new UserProfileVM(followingUserProfile.Id, followingUserProfile.UserName, followingUserProfile.DateCreated, usersProfilePictureVM));
            }
            return followingUserProfilesVM;
        }

        private UserProfile CreateNewUserProfile(Guid id)
        {
            return new UserProfile(id);
        }
    }
}