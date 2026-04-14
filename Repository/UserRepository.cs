using System.Text.Json;
using System.Text.Json.Nodes;
using Data;
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

        public async Task<User?> UpdateAsync(int id, JsonObject patchJson)
        {
            User? user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            var entityProps = typeof(User).GetProperties();

            foreach (var prop in entityProps)
            {
                if (!patchJson.TryGetPropertyValue(prop.Name, out var node))
                {
                    continue;
                }

                if (node == null)
                {
                    prop.SetValue(user, null);
                    continue;
                }

                var value = node.Deserialize(prop.PropertyType);
                prop.SetValue(user, value);
            }

            await _context.SaveChangesAsync();
            return user;
        }
    }
}
