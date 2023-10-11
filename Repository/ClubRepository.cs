using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;
using RunGroupSocialMedia.ViewModels;

namespace RunGroupSocialMedia.Services
{
	public class ClubRepository : IClubRepository
	{
        public AppDbContext _context { get; }

        public ClubRepository(AppDbContext context)
		{
            _context = context;
        }

        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public Club Add(CreateClubViewModel form, string imageUrl)
        {
            Club club = null;
            club = new Club
            {
                Title = form.Title,
                Description = form.Description,
                Image = imageUrl,
                AppUserId = form.AppUserId,
                Address = form.Address,
                ClubCategory = form.ClubCategory

            };
            Add(club);

            return club;
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _context.Clubs.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Club?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Clubs.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public Task<int> GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}

