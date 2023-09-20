using System;
using RunGroupSocialMedia.Models;
using RunGroupSocialMedia.ViewModels;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IRaceRepository
	{

        Task<IEnumerable<Race>> GetAll();

        Task<IEnumerable<Race>> GetAllRacesByCity(string city);

        Task<Race> GetByIdAsync(int id);

        Task<int> GetCountAsync();

        bool Add(Race race);
        Race Add(CreateRaceViewModel form, string imageUrl);

        bool Update(Race race);

        bool Delete(Race race);

        bool Save();

    }
}

