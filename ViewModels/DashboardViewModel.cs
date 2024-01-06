using System;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.ViewModels
{
	public class DashboardViewModel
	{
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public List<Race> Races { get; set; }
        public List<Club> Clubs { get; set; }

        public List<Race>? JoinedRaces { get; set; }
        public List<Club>? JoinedClubs { get; set; }
    }
}

