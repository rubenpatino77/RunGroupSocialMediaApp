using System;
using Microsoft.EntityFrameworkCore;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IFriendRepository
	{
        List<AppUser> GetUsersFriends(AppUser user);

        Task<AppUser> GetUserById(string id);
        
        ICollection<FriendRequest> GetUsersRecievedFriendRequests(AppUser user);
    }
}

