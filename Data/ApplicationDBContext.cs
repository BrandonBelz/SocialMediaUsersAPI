using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(
            DbContextOptions<ApplicationDBContext> dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<FriendRequest> FriendRequests { get; set; } = null!;
        public DbSet<Friendship> Friendships { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<FriendRequest>().HasKey(
                fr => new { fr.RequesterId, fr.RecipientId });

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Requester)
                .WithMany(u => u.SentRequests)
                .HasForeignKey(fr => fr.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Recipient)
                .WithMany(u => u.ReceivedRequests)
                .HasForeignKey(fr => fr.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>().HasKey(
                fr => new { fr.User1Id, fr.User2Id });

            modelBuilder.Entity<Friendship>()
                .HasOne(fr => fr.User1)
                .WithMany(u => u.FriendshipsAsUser1)
                .HasForeignKey(fr => fr.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(fr => fr.User2)
                .WithMany(u => u.FriendshipsAsUser2)
                .HasForeignKey(fr => fr.User2Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
