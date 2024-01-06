using System;
using Microsoft.EntityFrameworkCore;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Repository
{
	public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Club>> GetAllUserClubsByEmail(string userEmail)
        {

            var userClubs = _context.Clubs.Where(r => r.AppUser.Email == userEmail);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRacesByEmail(string userEmail)
        {
            var userRaces = _context.Races.Where(r => r.AppUser.Email == userEmail);
            return userRaces.ToList();
        }

        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
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

