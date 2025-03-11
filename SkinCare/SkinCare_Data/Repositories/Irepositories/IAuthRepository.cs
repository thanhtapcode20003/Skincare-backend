using SkinCare_Data.Data;
using System.Threading.Tasks;

namespace SkinCare_Data.IRepositories
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<int> GetUserCountByRoleAsync(int roleId);
        Task<User> GetUserByIdAsync(string userId);
        Task<bool> UserExistsAsync(string email);
        Task AddUserAsync(User user);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task SaveChangesAsync();
    }
}