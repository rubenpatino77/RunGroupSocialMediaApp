using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupSocialMedia.Models
{
	public class FriendRequest
	{
        public int Id { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        [InverseProperty("ReceivedFriendRequests")]
        public AppUser Receiver { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        [InverseProperty("SentFriendRequests")]
        public AppUser Sender { get; set; }

        public bool Accepted { get; set; }
    }
}

