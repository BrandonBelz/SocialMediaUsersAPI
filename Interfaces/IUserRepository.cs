using Models;
using Dtos;

namespace Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync(string? query);
        Task<User?> GetUserAsync(int id);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(int id, UpdateUserRequestDto userDto);
        Task<User?> DeleteAsync(int id);
    }
}
