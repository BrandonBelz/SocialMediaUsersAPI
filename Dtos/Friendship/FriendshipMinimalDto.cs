using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class FriendshipMinimalDto
    {
        [Required]
        public int User1Id { get; set; }

        [Required]
        public UserMinimizedDto User1 { get; set; } = null!;

        [Required]
        public int User2Id { get; set; }

        [Required]
        public UserMinimizedDto User2 { get; set; } = null!;

        [Required]
        public DateTime FriendsSince { get; set; }
    }
}
