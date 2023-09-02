using System;
using RunGroupSocialMedia.Data.Enum;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IClubRepository
	{

        Task<IEnumerable<Club>> GetAll();

        Task<IEnumerable<Club>> GetClubByCity(string city);

        Task<Club> GetByIdAsync(int id);

        Task<int> GetCountAsync();

        bool Add(Club club);

        bool Update(Club club);

        bool Delete(Club club);

        bool Save();

    }
}

