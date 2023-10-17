using System;
namespace RunGroupSocialMedia.ViewModels
{
	public class UserDetailViewModel
	{
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}

