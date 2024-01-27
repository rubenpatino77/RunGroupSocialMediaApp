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
using RunGroupSocialMedia.Services;
using RunGroupSocialMedia.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupSocialMedia.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IRaceRepository _raceRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboradRepository,
                                    IHttpContextAccessor contextAccessor,
                                    IPhotoService photoService,
                                    IUserRepository userRepository,
                                    IClubRepository clubRepository,
                                    IRaceRepository raceRepository,
                                    IFriendRepository friendRepository)
        {
            _dashboardRepository = dashboradRepository;
            _contextAccessor = contextAccessor;
            _photoService = photoService;
            _userRepository = userRepository;
            _clubRepository = clubRepository;
            _raceRepository = raceRepository;
            _friendRepository = friendRepository;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var userId = _contextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(userId);
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            string userEmail = _dashboardRepository.GetUserEmail(userId);
            List<Club> joinedClubs = _dashboardRepository.GetJoinedClubs(user);
            List<Race> joinedRaces = _dashboardRepository.GetJoinedRaces(user);
            List<AppUser> friends = _friendRepository.GetUsersFriends(user);
            var dashboardViewModel = new DashboardViewModel()
            {
                Email = userEmail,
                Races = userRaces,
                Clubs = userClubs,
                ProfileImageUrl = user.ProfileImageUrl,
                Pace = user.Pace,
                Mileage = user.Mileage,
                State = user.State,
                City = user.City,
                JoinedClubs = joinedClubs,
                JoinedRaces = joinedRaces,
                Friends = friends
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

        public void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, string photoUrl)
        {
            user.Id = editVM.id;
            user.City = editVM.City;
            user.State = editVM.State;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;
            user.ProfileImageUrl = photoUrl;
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

            if(editVM.Image != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult.Url.ToString());
                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            } else
            {
                var photoUrl = user.ProfileImageUrl;
                MapUserEdit(user, editVM, photoUrl);
                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            }

            /*FOR AZURE
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

        [HttpPost]
        public async Task<IActionResult> LeaveClub(int clubId)
        {
            AppUser user = await _userRepository.GetUserById(_contextAccessor.HttpContext.User.GetUserId());
            Club club = await _clubRepository.GetByIdAsync(clubId);
            bool work = _clubRepository.RemoveClubMember(club, user);
            bool userRemoveResult = _userRepository.LeaveClub(club, user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> LeaveRace(int raceId)
        {
            AppUser user = await _userRepository.GetUserById(_contextAccessor.HttpContext.User.GetUserId());
            Race race = await _raceRepository.GetByIdAsync(raceId);
            bool work = _raceRepository.RemoveRaceMember(race, user);
            return RedirectToAction("Index");
        }
    }
}

