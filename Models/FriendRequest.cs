namespace Models
{
    public class FriendRequest
    {
        public int RequesterId { get; set; }
        public User Requester { get; set; } = null!;

        public int RecipientId { get; set; }
        public User Recipient { get; set; } = null!;

        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}
