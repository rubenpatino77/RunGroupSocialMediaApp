using System;
using RunGroupSocialMedia.Data.Enum;
using RunGroupSocialMedia.Models;
using RunGroupSocialMedia.ViewModels;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IClubRepository
	{

        Task<IEnumerable<Club>> GetAll();

        Task<IEnumerable<Club>> GetClubByCity(string city);

        Task<Club> GetByIdAsync(int id);
        Task<Club?> GetByIdAsyncNoTracking(int id);

        Task<int> GetCountAsync();

        bool Add(Club club);
        Club Add(CreateClubViewModel form, string imageUrl);

        bool Update(Club club);

        bool Delete(Club club);

        bool Save();

    }
}

