using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace RunGroupSocialMedia.Models
{
	public class AppUser : IdentityUser
	{
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        [InverseProperty("AppUser")]
        public ICollection<Club> Clubs { get; set; }
        public ICollection<Race> Races { get; set; }

        public ICollection<AppUser>? Friends { get; set; }

        [InverseProperty("Sender")]
        public ICollection<FriendRequest>? SentFriendRequests { get; set; }
        [InverseProperty("Receiver")]
        public ICollection<FriendRequest>? ReceivedFriendRequests { get; set; }

        [InverseProperty("RaceMembers")]
        public ICollection<Race>? JoinedRaces { get; set; }
        [InverseProperty("ClubMembers")]
        public ICollection<Club>? JoinedClubs { get; set; }


    }
}

