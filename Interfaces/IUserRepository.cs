using System;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IUserRepository
	{
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}

