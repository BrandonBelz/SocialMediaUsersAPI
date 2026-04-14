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
    }
}
