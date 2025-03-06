using SkinCare_Data.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Data.IRepositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(string userId);
        Task<User> FindByEmailAsync(string email);
        Task<Role> FindRoleByIdAsync(int roleId);
        Task<int> GetUserCountByRoleAsync(int roleId);
        Task<bool> UserExistsAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task SaveChangesAsync();
    }
}