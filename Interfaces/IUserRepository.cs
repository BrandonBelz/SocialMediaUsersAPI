using System.Text.Json.Nodes;
using Models;

namespace Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync(string? query);
        Task<User?> GetUserAsync(int id);
        Task<User?> GetUserAsync(string username);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(int id, JsonObject patchJson);
        Task<User?> DeleteAsync(int id);

        Task<List<User>?> GetFriendsAsync(int id);
        Task<Friendship?> GetFriendshipAsync(int user1Id, int user2Id);
        Task<Friendship?> RemoveFriend(int id, int friendId);

        Task<FriendRequest?> GetFriendRequestAsync(int user1Id, int user2Id);
        Task<List<User>?> GetSentRequestsAsync(int id);
        Task<List<User>?> GetReceivedRequestsAsync(int id);
        Task<FriendRequest?> SendRequest(int from, int to);
        Task<Friendship?> AcceptRequest(int receiver, int requester);
        Task<FriendRequest?> DenyRequest(int receiver, int requester);
    }
}
