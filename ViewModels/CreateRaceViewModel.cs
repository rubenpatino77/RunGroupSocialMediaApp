using System;
using RunGroupSocialMedia.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.ViewModels
{
	public class CreateRaceViewModel
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public RaceCategory RaceCategory { get; set; }

        public Photo photo { get; set; }

        public string AppUserId { get; set; }
    }
}

