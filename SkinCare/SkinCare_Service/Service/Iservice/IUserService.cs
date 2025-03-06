using SkinCare_Data.Data;
using SkinCare_Data.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Service.IService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string userId);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(string userId, UpdateUserRequest updateUserRequest);
        Task<bool> DeleteUserAsync(string userId);
    }
}