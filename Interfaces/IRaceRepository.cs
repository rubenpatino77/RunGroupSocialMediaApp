using System;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IRaceRepository
	{

        Task<IEnumerable<Race>> GetAll();

        Task<IEnumerable<Race>> GetAllRacesByCity(string city);

        Task<Race> GetByIdAsync(int id);

        Task<int> GetCountAsync();

        bool Add(Race race);

        bool Update(Race race);

        bool Delete(Race race);

        bool Save();

    }
}

