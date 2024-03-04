using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;
using RunGroupSocialMedia.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupSocialMedia.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFriendRepository _friendRepository;

        public UserController(IUserRepository userRepository, UserManager<AppUser> userManager, IFriendRepository friendRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _friendRepository = friendRepository;
        }

        [HttpGet("users")]
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    Pace = user.Pace,
                    //City = user.City,
                    //State = user.State,
                    Mileage = user.Mileage,
                    UserName = user.UserName,
                    ProfileImageUrl = user.ProfileImageUrl,
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            //var user = await _userRepository.GetUserById(id);
            AppUser user = await _friendRepository.GetUserByIdIncludeRecievedRequests(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            var userRaces = await _userRepository.GetAllUserRacesByEmail(user.Email);
            var userClubs = await _userRepository.GetAllUserClubsByEmail(user.Email);
            bool isFriend = _friendRepository.IsFriend(user.Id);
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = id,
                Pace = user.Pace,
                City = user.City,
                State = user.State,
                Mileage = user.Mileage,
                UserName = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl,
                Races = userRaces,
                Clubs = userClubs,
                IsFriend = isFriend
            };
            return View(userDetailViewModel);
        }

        [HttpPost]
        public IActionResult SendFriendRequest(string id)
        {
            _friendRepository.SendFriendRequest(id);

            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public IActionResult RemoveFriend(string usersId)
        {
            _friendRepository.RemoveFriend(usersId);

            return RedirectToAction("Detail", usersId);
        }

        //todo check if addfriend works and create removefriend
    }
}

