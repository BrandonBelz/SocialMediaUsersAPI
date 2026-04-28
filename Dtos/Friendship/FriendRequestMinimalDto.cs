namespace Dtos
{
    public class FriendRequestMinimalDto
    {
        public int RequesterId { get; set; }
        public UserMinimizedDto Requester { get; set; } = null!;

        public int RecipientId { get; set; }
        public UserMinimizedDto Recipient { get; set; } = null!;

        public DateTime SentAt { get; set; }
    }
}
