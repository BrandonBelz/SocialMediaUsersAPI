using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Data;
using Dtos;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync(string? query)
        {
            IQueryable<User> users = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                users = users.Where(s => s.Username.Contains(query));
            }

            return await users.ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteAsync(int id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> UpdateAsync(int id, UpdateUserRequestDto requestDto)
        {
            User? user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            if (requestDto.Biography != null)
            {
                user.Biography = requestDto.Biography;
            }

            if (requestDto.Email != null)
            {
                user.Email = requestDto.Email;
            }

            if (requestDto.Username != null)
            {
                user.Username = requestDto.Username;
            }

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>?> GetFriendsAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            return user.Friends;
        }

        public async Task<Friendship?> RemoveFriend(int id, int friendId)
        {
            Friendship? friendship = await _context.Friendships.FirstOrDefaultAsync(x =>
                (x.User1Id == id && x.User2Id == friendId)
                || (x.User2Id == id && x.User1Id == friendId)
            );
            if (friendship == null)
            {
                return null;
            }
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
            return friendship;
        }

        public async Task<List<User>?> GetSentRequestsAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            return user.SentRequests.Select(f => f.Recipient).ToList();
        }

        public async Task<List<User>?> GetReceivedRequestsAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            return user.ReceivedRequests.Select(f => f.Requester).ToList();
        }

        public async Task<FriendRequest?> SendRequest(int from, int to)
        {
            User? sender = await _context.Users.FindAsync(from);
            User? receiver = await _context.Users.FindAsync(to);

            if (sender == null || receiver == null)
            {
                return null;
            }

            var newRequest = new FriendRequest { RequesterId = from, RecipientId = to };
            await _context.FriendRequests.AddAsync(newRequest);
            await _context.SaveChangesAsync();

            return newRequest;
        }

        public async Task<Friendship?> AcceptRequest(int receiver, int requester)
        {
            FriendRequest? request = await _context.FriendRequests.FirstOrDefaultAsync(x =>
                x.RecipientId == receiver && x.RequesterId == requester
            );

            if (request == null)
            {
                return null;
            }

            var friendship = new Friendship { User1Id = requester, User2Id = receiver };

            await _context.AddAsync(friendship);
            _context.Remove(request);
            await _context.SaveChangesAsync();

            return friendship;
        }

        public async Task<FriendRequest?> DenyRequest(int receiver, int requester)
        {
            FriendRequest? request = await _context.FriendRequests.FirstOrDefaultAsync(x =>
                x.RecipientId == receiver && x.RequesterId == requester
            );

            if (request == null)
            {
                return null;
            }

            _context.FriendRequests.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<Friendship?> GetFriendshipAsync(int user1Id, int user2Id)
        {
            return await _context.Friendships.FirstOrDefaultAsync(f =>
                (f.User1Id == user1Id && f.User2Id == user2Id)
                || (f.User1Id == user2Id && f.User2Id == user1Id)
            );
        }

        public async Task<FriendRequest?> GetFriendRequestAsync(int user1Id, int user2Id)
        {
            return await _context.FriendRequests.FirstOrDefaultAsync(f =>
                (f.RecipientId == user1Id && f.RequesterId == user2Id)
                || (f.RecipientId == user2Id && f.RequesterId == user1Id)
            );
        }
    }
}
