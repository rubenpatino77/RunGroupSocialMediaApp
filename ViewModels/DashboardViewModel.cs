using System;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.ViewModels
{
	public class DashboardViewModel
	{
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public List<Race> Races { get; set; }
        public List<Club> Clubs { get; set; }
    }
}

