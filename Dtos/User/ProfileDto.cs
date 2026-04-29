using Models;

namespace Dtos
{
    public enum Relationship
    {
        Stranger,
        Friend,
        RequestSent,
        RequestReceived,
    }

    public class ProfileDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = String.Empty;

        public string Biography { get; set; } = String.Empty;

        public DateTime JoinedOn { get; set; } = DateTime.Now;

        private List<User> Friends { get; set; }
        private List<FriendRequest> SentRequests { get; set; }
        private List<FriendRequest> ReceivedRequests { get; set; }
        private int ViewerId { get; set; }

        public int NumFriends => Friends.Count();

        public Relationship RelationshipToUser => GetRelationship();

        public ProfileDto(User user, int viewerId)
        {
            Id = user.Id;
            Username = user.Username;
            Biography = user.Biography;
            JoinedOn = user.JoinedOn;
            Friends = user.Friends;
            SentRequests = user.SentRequests;
            ReceivedRequests = user.ReceivedRequests;
            ViewerId = viewerId;
        }

        private Relationship GetRelationship()
        {
            if (Friends.Select(u => u.Id == ViewerId).Count() > 0)
            {
                return Relationship.Friend;
            }

            if (SentRequests.Select(f => f.RecipientId == ViewerId).Count() > 0)
            {
                return Relationship.RequestReceived;
            }

            if (ReceivedRequests.Select(f => f.RequesterId == ViewerId).Count() > 0)
            {
                return Relationship.RequestSent;
            }

            return Relationship.Stranger;
        }
    }
}
