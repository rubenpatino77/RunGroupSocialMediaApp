using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;
using RunGroupSocialMedia.Repository;
using RunGroupSocialMedia.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupSocialMedia.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPhotoService _photoService;
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboradRepository, IHttpContextAccessor contextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboradRepository;
            _contextAccessor = contextAccessor;
            _photoService = photoService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            string userEmail = _dashboardRepository.GetUserEmail(_contextAccessor.HttpContext.User.GetUserId());
            var dashboardViewModel = new DashboardViewModel()
            {
                Email = userEmail,
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditProfile()
        {
            var curUserId = _contextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);

            if (user == null)
            {
                return View("Error");
            }

            var editMV = new EditUserDashboardViewModel()
            {
                id = curUserId,
                City = user.City,
                State = user.State,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
            };
            return View(editMV);
        }

        public void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult uploadResult)
        {
            user.Id = editVM.id;
            user.City = editVM.City;
            user.State = editVM.State;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;
            user.ProfileImageUrl = uploadResult.Url.ToString();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditProfile", editVM);
            }

            var user = await _dashboardRepository.GetByIdNoTracking(editVM.id);

            if(user.ProfileImageUrl == null || user.ProfileImageUrl == "")
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            } else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                } catch(Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            }






            /*
            if (user == null)
            {
                return View("Error");
            }

            if (editVM.Image != null) // only update profile image
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Failed to upload image");
                    return View("EditProfile", editVM);
                }

                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                user.ProfileImageUrl = photoResult.Url.ToString();
                editVM.ProfileImageUrl = user.ProfileImageUrl;

                await .UpdateAsync(user);

                return View(editVM);
            }

            user.City = editVM.City;
            user.State = editVM.State;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Detail", "User", new { user.Id });*/
        }
    }
}

