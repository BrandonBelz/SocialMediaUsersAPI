namespace Models
{
    public class Friendship
    {
        public int User1Id { get; set; }
        public User User1 { get; set; } = null!;

        public int User2Id { get; set; }
        public User User2 { get; set; } = null!;

        public DateTime FriendsSince { get; set; } = DateTime.Now;
    }
}
