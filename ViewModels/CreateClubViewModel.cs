using System;
using RunGroupSocialMedia.Data.Enum;
using RunGroupSocialMedia.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupSocialMedia.ViewModels
{
	public class CreateClubViewModel
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Address? Address { get; set; }
        public ClubCategory ClubCategory { get; set; }

        public Photo photo { get; set; }

        public string AppUserId { get; set; }
    }
}

