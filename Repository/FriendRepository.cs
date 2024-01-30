using System;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Repository
{
	public class FriendRepository : IFriendRepository
	{
        private readonly AppDbContext _context;

        public FriendRepository(AppDbContext context)
		{
            _context = context;
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
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
	}
}

