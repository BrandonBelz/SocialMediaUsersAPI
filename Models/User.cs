using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = String.Empty;

        public string Biography { get; set; } = String.Empty;

        public DateTime JoinedOn { get; set; } = DateTime.Now;

        public List<FriendRequest> SentRequests { get; set; } =
            new List<FriendRequest>();
        public List<FriendRequest> ReceivedRequests { get; set; } =
            new List<FriendRequest>();

        public List<Friendship> FriendshipsAsUser1 { get; set; } =
            new List<Friendship>();
        public List<Friendship> FriendshipsAsUser2 { get; set; } =
            new List<Friendship>();

        [NotMapped]
        public List<User> Friends =>
            (List<User>)FriendshipsAsUser1.Select(f => f.User2)
                .Concat(FriendshipsAsUser2.Select(f => f.User1));
    }
}
