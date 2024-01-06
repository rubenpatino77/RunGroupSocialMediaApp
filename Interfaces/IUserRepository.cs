using System;
using Microsoft.EntityFrameworkCore;
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
        Task<List<Club>> GetAllUserClubsByEmail(string userEmail);
        Task<List<Race>> GetAllUserRacesByEmail(string userEmail);

        bool JoinClub(Club club, AppUser user);
        bool LeaveClub(Club club, AppUser user);

        bool JoinRace(Race race, AppUser user);
        bool LeaveRace(Race race, AppUser user);
    }
}

