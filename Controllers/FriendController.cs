using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;
using RunGroupSocialMedia.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupSocialMedia.Controllers
{
    public class FriendController : Controller
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public FriendController(IHttpContextAccessor contextAccessor, IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
            _contextAccessor = contextAccessor;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var user = _friendRepository.GetUserById(_contextAccessor.HttpContext.User.GetUserId());
            List<AppUser> usersFriends = _friendRepository.GetUsersFriends(user.Result);

            FriendViewModel viewModel = new FriendViewModel()
            {
                FriendsList = usersFriends
            };
            return View(viewModel);
        }

        public IActionResult FriendRequest()
        {
            var user = _friendRepository.GetUserById(_contextAccessor.HttpContext.User.GetUserId());
            var friendRequests = _friendRepository.GetUsersRecievedFriendRequests(user.Result);
            FriendRequestViewModel requests = new FriendRequestViewModel
            {
                FriendRequests = friendRequests.ToList()
            };

            return View(requests);
        }
    }
}

