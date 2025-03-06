using Microsoft.EntityFrameworkCore;
using SkinCare_Data.Data;
using SkinCare_Data.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SkinCare_DBContext _context;

        public UserRepository(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.SkinType)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.SkinType)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Role> FindRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<int> GetUserCountByRoleAsync(int roleId)
        {
            return await _context.Users
                .CountAsync(u => u.RoleId == roleId);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}