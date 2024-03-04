using System;
using Microsoft.EntityFrameworkCore;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IFriendRepository
	{
        List<AppUser> GetUsersFriends(AppUser user);

        Task<AppUser> GetUserById(string id);

        Task<AppUser> GetUserByIdIncludeSentRequests(string id);

        Task<AppUser> GetUserByIdIncludeRecievedRequests(string id);

        ICollection<FriendRequest> GetUsersRecievedFriendRequests(AppUser user);

        bool IsFriend(string UserId);

        bool Save();

        bool SendFriendRequest(string userId);

        bool ConfirmAddFriend(string userId);

        bool RemoveFriend(string userId);

        bool CancelFriendRequest(string userId);
    }
}

