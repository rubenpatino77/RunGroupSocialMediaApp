using System;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IDashboardRepository
	{
        string GetUserEmail(string id);

        Task<List<Club>> GetAllUserClubs();

        Task<List<Race>> GetAllUserRaces();
        
        Task<AppUser> GetUserById(string id);
        
        Task<AppUser> GetByIdNoTracking(string id);

        bool Update(AppUser user);

        bool Save();

        List<Club> GetJoinedClubs(AppUser user);
        bool JoinClub(Club club, AppUser user);
        bool LeaveClub(Club club, AppUser user);

        List<Race> GetJoinedRaces(AppUser user);
        bool JoinRace(Race race, AppUser user);
        bool LeaveRace(Race race, AppUser user);
    }
}

