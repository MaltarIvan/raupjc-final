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
using PictureGallery.Models.Shared;
using Microsoft.Extensions.Configuration;
using PictureGallery.Utilities;

namespace PictureGallery.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGalleryRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _storageAccountName;
        private readonly string _storageAccountKey;
        private readonly string _storageContainerName;

        public MainController(IGalleryRepository repository, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _userManager = userManager;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _storageAccountName = configuration["StorageAccountSettings:StorageAccountName"];
            _storageAccountKey = configuration["StorageAccountSettings:StorageAccountKey1"];
            _storageContainerName = configuration["StorageAccountSettings:ResourceGroup"];

        }

        public object GallerySqlRepository { get; private set; }
        public Picture Picture { get; private set; }

        public async Task<IActionResult> Index()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            List<string> roles = (List<string>) await _userManager.GetRolesAsync(applicationUser);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            if (currentUser == null)
            {
                return RedirectToAction("MakeNewProfile");
            }
            List<Picture> pictures = await _repository.GetAllPicturesAsync();

            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser);
            List<PictureVM> picturesToPresent = new List<PictureVM>();
            foreach (var item in pictures)
            {
                picturesToPresent.Add(new PictureVM(item));
            }

            // get all users
            List<UserProfileVM> userProfilesVM = await GetAllUsersVMAsync(currentUser.Id);

            //get following users
            List<UserProfileVM> followingUserProfilesVM = GetFollowingUsersVM(currentUser);

            MainVM indexViewModel = new MainVM("Newest Pictures", userProfileVM, picturesToPresent, userProfilesVM, followingUserProfilesVM);

            return View("Index", indexViewModel);
        }

        public async Task<ActionResult> MakeNewProfile()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid curretUserId = new Guid(applicationUser.Id);
            if (await _repository.ContainsUserAsync(curretUserId))
            {
                return RedirectToAction("Index");
            }
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
                    ModelState.AddModelError("CustomError", "Please choose either a GIF, JPG or PNG image.");
                    model.ProfilePictureUpload = null;
                    return View(model);
                }
            }
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                Guid curretUserId = new Guid(applicationUser.Id);
                if (await _repository.ContainsUserAsync(curretUserId))
                {
                    return RedirectToAction("Index");
                }
                Picture profilePicture = null;
                byte[] data = null;

                if (model.ProfilePictureUpload != null && model.ProfilePictureUpload.Length > 0)
                {
                    BinaryReader reader = new BinaryReader(model.ProfilePictureUpload.OpenReadStream());
                    data = reader.ReadBytes((int)model.ProfilePictureUpload.Length);

                    var uploader = new AzureStorageUtility(_storageAccountName, _storageAccountKey);
                    profilePicture = await uploader.Upload(_storageContainerName, data);

                    profilePicture.UserId = curretUserId;
                }
                else
                {
                    var webRoot = _hostingEnvironment.WebRootPath;
                    var file = Path.Combine(webRoot, "Content\\default-profile-picture.png");
                    data = System.IO.File.ReadAllBytes(file);

                    var uploader = new AzureStorageUtility(_storageAccountName, _storageAccountKey);
                    profilePicture = await uploader.Upload(_storageContainerName, data);

                    profilePicture.UserId = curretUserId;
                }
                
                UserProfile currentUser = CreateNewUserProfile(curretUserId);

                if (profilePicture != null)
                {
                    profilePicture.User = currentUser;
                    currentUser.ProfilePicture = profilePicture;
                }

                currentUser.UserName = model.UserName;
                await _repository.AddUserAsync(currentUser);

                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Top()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            if (currentUser == null)
            {
                return RedirectToAction("MakeNewProfile");
            }
            List<Picture> pictures = await _repository.GetAllPicturesAsync();
            pictures = pictures.OrderByDescending(p => p.NumberOfLikes).ToList();

            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser);
            List<PictureVM> picturesToPresent = new List<PictureVM>();
            foreach (var item in pictures)
            {
                picturesToPresent.Add(new PictureVM(item));
            }

            // get all users
            List<UserProfileVM> userProfilesVM = await GetAllUsersVMAsync(currentUser.Id);

            //get following users
            List<UserProfileVM> followingUserProfilesVM = GetFollowingUsersVM(currentUser);

            MainVM indexViewModel = new MainVM("Top Pictures", userProfileVM, picturesToPresent, userProfilesVM, followingUserProfilesVM);

            return View("Index", indexViewModel);
        }

        public async Task<IActionResult> Favorites()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            if (currentUser == null)
            {
                return RedirectToAction("MakeNewProfile");
            }
            List<Picture> pictures = currentUser.Favorites;

            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser);
            List<PictureVM> picturesToPresent = new List<PictureVM>();
            foreach (var item in pictures)
            {
                picturesToPresent.Add(new PictureVM(item));
            }

            // get all users
            List<UserProfileVM> userProfilesVM = await GetAllUsersVMAsync(currentUser.Id);

            //get following users
            List<UserProfileVM> followingUserProfilesVM = GetFollowingUsersVM(currentUser);

            MainVM indexViewModel = new MainVM("Your Favorites", userProfileVM, picturesToPresent, userProfilesVM, followingUserProfilesVM);
            
            return View("Index", indexViewModel);
        }

        public async Task<IActionResult> FollowingPictures()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            if (currentUser == null)
            {
                return RedirectToAction("MakeNewProfile");
            }
            List<Picture> pictures = new List<Picture>();
            foreach (var user in currentUser.Following)
            {
                pictures.AddRange(await _repository.GetAllPicturesFromUserAsync(user.Id));
            }

            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser);
            List<PictureVM> picturesToPresent = new List<PictureVM>();
            foreach (var item in pictures)
            {
                picturesToPresent.Add(new PictureVM(item));
            }

            // get all users
            List<UserProfileVM> userProfilesVM = await GetAllUsersVMAsync(currentUser.Id);

            //get following users
            List<UserProfileVM> followingUserProfilesVM = GetFollowingUsersVM(currentUser);

            MainVM indexViewModel = new MainVM("Pictures You Follow", userProfileVM, picturesToPresent, userProfilesVM, followingUserProfilesVM);

            return View("Index", indexViewModel);
        }

        public async Task<IActionResult> HotPictures()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile currentUser = await _repository.GetUserByIdAsync(new Guid(applicationUser.Id));
            if (currentUser == null)
            {
                return RedirectToAction("MakeNewProfile");
            }
            List<Picture> pictures = await _repository.GetHotPicturesAsync();

            ProfilePictureVM profilePictureVM = new ProfilePictureVM(currentUser.ProfilePicture);
            UserProfileVM userProfileVM = new UserProfileVM(currentUser);
            List<PictureVM> picturesToPresent = new List<PictureVM>();
            foreach (var item in pictures)
            {
                picturesToPresent.Add(new PictureVM(item));
            }

            // get all users
            List<UserProfileVM> userProfilesVM = await GetAllUsersVMAsync(currentUser.Id);

            //get following users
            List<UserProfileVM> followingUserProfilesVM = GetFollowingUsersVM(currentUser);

            MainVM indexViewModel = new MainVM("Hot pictures", userProfileVM, picturesToPresent, userProfilesVM, followingUserProfilesVM);

            return View("Index", indexViewModel);
        }

        public async Task<List<UserProfileVM>> GetAllUsersVMAsync(Guid id)
        {
            List<UserProfile> userProfiles = await _repository.GetUserProfilesAsync(id);
            List<UserProfileVM> userProfilesVM = new List<UserProfileVM>();
            foreach (var userProfile in userProfiles)
            {
                ProfilePictureVM usersProfilePictureVM = new ProfilePictureVM(userProfile.ProfilePicture);
                userProfilesVM.Add(new UserProfileVM(userProfile));
            }
            return userProfilesVM;
        }

        public List<UserProfileVM> GetFollowingUsersVM(UserProfile currentUser)
        {
            List<UserProfile> followingUserProfiles = currentUser.Following;
            List<UserProfileVM> followingUserProfilesVM = new List<UserProfileVM>();
            foreach (var followingUserProfile in followingUserProfiles)
            {
                ProfilePictureVM usersProfilePictureVM = new ProfilePictureVM(followingUserProfile.ProfilePicture);
                followingUserProfilesVM.Add(new UserProfileVM(followingUserProfile));
            }
            return followingUserProfilesVM;
        }

        private UserProfile CreateNewUserProfile(Guid id)
        {
            return new UserProfile(id);
        }
    }
}