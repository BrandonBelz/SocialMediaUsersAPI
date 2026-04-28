namespace Dtos
{
    public class FriendshipMinimalDto
    {
        public int User1Id { get; set; }
        public UserMinimizedDto User1 { get; set; } = null!;
        public int User2Id { get; set; }
        public UserMinimizedDto User2 { get; set; } = null!;
        public DateTime FriendsSince { get; set; }
    }
}
