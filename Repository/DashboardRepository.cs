using System;
using Microsoft.EntityFrameworkCore;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Repository
{
	public class DashboardRepository : IDashboardRepository
	{
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(r => r.AppUser.Id == curUser);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = _context.Races.Where(r => r.AppUser.Id == curUser);
            return userRaces.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        
        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public string GetUserEmail(string id)
        {
            return GetUserById(id).Result.Email.ToString();
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }
        
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public List<Club> GetJoinedClubs(AppUser user)
        {
            var joinedClubs = _context.Clubs.Where(c => c.ClubMembers.Any(cm => cm.Email == user.Email)).ToList();
            return joinedClubs;
        }

        public bool JoinClub(Club club, AppUser user)
        {
            user.JoinedClubs.Add(club);
            return Save();
        }

        public bool LeaveClub(Club club, AppUser user)
        {
            user.JoinedClubs.Remove(club);
            return Save();
        }

        public List<Race> GetJoinedRaces(AppUser user)
        {
            var joinedRaces = _context.Races.Where(r => r.RaceMembers.Any(rm => rm.Email == user.Email)).ToList();
            return joinedRaces;
        }

        public bool JoinRace(Race race, AppUser user)
        {
            user.JoinedRaces.Add(race);
            return Save();
        }

        public bool LeaveRace(Race race, AppUser user)
        {
            user.JoinedRaces.Remove(race);
            return Save();
        }
    }
}

