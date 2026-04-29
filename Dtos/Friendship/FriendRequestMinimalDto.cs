using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class FriendRequestMinimalDto
    {
        [Required]
        public int RequesterId { get; set; }

        [Required]
        public UserMinimizedDto Requester { get; set; } = null!;

        [Required]
        public int RecipientId { get; set; }

        [Required]
        public UserMinimizedDto Recipient { get; set; } = null!;

        [Required]
        public DateTime SentAt { get; set; }
    }
}
