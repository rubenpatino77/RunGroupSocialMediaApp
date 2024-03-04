using System;
using Microsoft.EntityFrameworkCore;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Repository
{
	public class FriendRepository : IFriendRepository
	{
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;

        public FriendRepository(AppDbContext context, IHttpContextAccessor httpContext, IUserRepository userRepository)
		{
            _context = context;
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByIdIncludeSentRequests(string id)
        {
            return await _context.Users.Include(u => u.SentFriendRequests).ThenInclude(fr => fr.Receiver).FirstOrDefaultAsync(u => u.Id == id); ;
        }

        public async Task<AppUser> GetUserByIdIncludeRecievedRequests(string id)
        {
            return await _context.Users.Include(u => u.ReceivedFriendRequests).ThenInclude(fr => fr.Sender).FirstOrDefaultAsync(u => u.Id == id);
        }

        public List<AppUser> GetUsersFriends(AppUser user)
		{
			List<AppUser> friendsList;
			ICollection<AppUser> friends = user.Friends;
			if(friends != null)
			{
				friendsList = friends.ToList();
			} else
			{
				friendsList = new List<AppUser>();
			}

			return friendsList;
		}


		public ICollection<FriendRequest> GetUsersRecievedFriendRequests(AppUser user)
		{

			ICollection<FriendRequest> requests = user.ReceivedFriendRequests;

			if(requests == null)
			{
				requests = new List<FriendRequest>();
			} else
			{
				requests = requests.ToList();
			}

			return requests;
		}

		public bool IsFriend(string userId)
		{
			bool isUserFriend = false;
			var curUser = _context.Users.FirstOrDefault(user => user.Id == _httpContext.HttpContext.User.GetUserId());
			var otherUser = _context.Users.FirstOrDefault(user => user.Id == userId);
			if (curUser.Friends != null && curUser.Friends.Contains(otherUser))
			{
				isUserFriend = true;
			}

			return isUserFriend;
		}

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SendFriendRequest(string userId)
        {
            //AppUser curUser = _userRepository.GetUserById(_httpContext.HttpContext.User.GetUserId()).Result;
            var curUser = _context.Users.FirstOrDefaultAsync(user => user.Id == _httpContext.HttpContext.User.GetUserId()).Result;
            var otherUser = _context.Users.FirstOrDefaultAsync(user => user.Id == userId).Result;
            //AppUser otherUser = _userRepository.GetUserById(userId).Result;

            FriendRequest friendRequest = new FriendRequest
            {
                SenderId = curUser.Id.ToString(),
                Sender = curUser,
                ReceiverId = userId,
                Receiver = otherUser,
                Accepted = false
            };

            if (curUser.SentFriendRequests == null)
            {
                curUser.SentFriendRequests = new List<FriendRequest>();
            }

            if (otherUser.ReceivedFriendRequests == null)
            {
                otherUser.ReceivedFriendRequests = new List<FriendRequest>();
            }

            curUser.SentFriendRequests.Add(friendRequest);
            otherUser.ReceivedFriendRequests.Add(friendRequest);

            _context.Update(curUser);
            _context.Update(otherUser);

            return Save();
        }

        public bool ConfirmAddFriend(string userId)
		{
            var curUser = _context.Users.FirstOrDefault(user => user.Id == _httpContext.HttpContext.User.GetUserId());
            var otherUser = _context.Users.FirstOrDefault(user => user.Id == userId);

			curUser.Friends.Add(otherUser);
			otherUser.Friends.Add(curUser);

			return Save();
        }

        public bool RemoveFriend(string userId)
        {
            var curUser = _context.Users.FirstOrDefault(user => user.Id == _httpContext.HttpContext.User.GetUserId());
            var otherUser = _context.Users.FirstOrDefault(user => user.Id == userId);

			curUser.Friends.Remove(otherUser);
			otherUser.Friends.Remove(curUser);

            return Save();
        }

		public bool CancelFriendRequest(string userId)
		{
            var curUser = _context.Users.FirstOrDefault(user => user.Id == _httpContext.HttpContext.User.GetUserId());
            var otherUser = _context.Users.FirstOrDefault(user => user.Id == userId);

			FriendRequest friendRequest = curUser.SentFriendRequests.FirstOrDefault(fRequest => fRequest.SenderId == curUser.Id && fRequest.ReceiverId == otherUser.Id);

			curUser.SentFriendRequests.Remove(friendRequest);
			otherUser.ReceivedFriendRequests.Remove(friendRequest);
            return Save();
		}
    }
}

