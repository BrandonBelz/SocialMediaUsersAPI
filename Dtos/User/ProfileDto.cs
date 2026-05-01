using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Models;

namespace Dtos
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Relationship
    {
        Stranger,
        Friend,
        RequestSent,
        RequestReceived,
    }

    public class ProfileDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = String.Empty;

        [Required]
        public string Biography { get; set; } = String.Empty;

        [Required]
        public DateTime JoinedOn { get; set; } = DateTime.Now;

        [Required]
        public string? ProfilePicUrl { get; set; }

        private List<User> Friends { get; set; }
        private List<FriendRequest> SentRequests { get; set; }
        private List<FriendRequest> ReceivedRequests { get; set; }
        private int ViewerId { get; set; }

        [Required]
        public int NumFriends => Friends.Count();

        [Required]
        public Relationship RelationshipToUser => GetRelationship();

        public ProfileDto(User user, int viewerId)
        {
            Id = user.Id;
            Username = user.Username;
            Biography = user.Biography;
            JoinedOn = user.JoinedOn;
            ProfilePicUrl = user.ProfilePicUrl;
            Friends = user.Friends;
            SentRequests = user.SentRequests;
            ReceivedRequests = user.ReceivedRequests;
            ViewerId = viewerId;
        }

        private Relationship GetRelationship()
        {
            if (Friends.Any(u => u.Id == ViewerId))
            {
                return Relationship.Friend;
            }

            if (SentRequests.Any(f => f.RecipientId == ViewerId))
            {
                return Relationship.RequestReceived;
            }

            if (ReceivedRequests.Any(f => f.RequesterId == ViewerId))
            {
                return Relationship.RequestSent;
            }

            return Relationship.Stranger;
        }
    }
}
