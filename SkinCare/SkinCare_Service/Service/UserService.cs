using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCare_Data;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.User;
using System.Threading.Tasks;

public class UserService
{
    private readonly SkinCare_DBContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(SkinCare_DBContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Get all users
    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all users");

            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.SkinType)
                .ToListAsync();

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching users: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    // Get user by ID
    public async Task<User> GetUserByIdAsync(string userId)
    {
        try
        {
            _logger.LogInformation("Fetching user with ID: {UserId}", userId);

            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.SkinType)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return null;
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user with ID {UserId}: {ErrorMessage}", userId, ex.Message);
            throw;
        }
    }

    // Create user
    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            _logger.LogInformation("Creating new user with email: {Email}", user.Email);

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new Exception("Email already exists!");
            }

            var role = await _context.Roles.FindAsync(user.RoleId);
            if (role == null)
            {
                throw new Exception("Role not found!");
            }

            int userCount = await _context.Users.CountAsync(u => u.RoleId == role.RoleId);
            string userIdPrefix = role.RoleId switch
            {
                1 => "C", // Customer
                2 => "S", // Staff
                3 => "M", // Manager
                _ => throw new Exception("Invalid role ID!")
            };

            user.UserId = $"{userIdPrefix}{(userCount + 1):D3}";
            user.CreateAt = DateTime.UtcNow;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash password

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user with email {Email}: {ErrorMessage}", user.Email, ex.Message);
            throw;
        }
    }

    // Update user (only username, email, phoneNumber, address) using DTO, without updating UserId
    public async Task<User> UpdateUserAsync(string userId, UpdateUserRequest updateUserRequest)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {UserId}", userId);

            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
            {
                throw new Exception("User not found!");
            }

            // Chỉ cập nhật các trường được phép từ DTO: userName, email, phoneNumber, address
            existingUser.UserName = updateUserRequest.UserName ?? existingUser.UserName;
            if (updateUserRequest.Email != null && updateUserRequest.Email != existingUser.Email)
            {
                if (await _context.Users.AnyAsync(u => u.Email == updateUserRequest.Email && u.UserId != userId))
                {
                    throw new Exception("Email already exists for another user!");
                }
                existingUser.Email = updateUserRequest.Email;
            }
            existingUser.PhoneNumber = updateUserRequest.PhoneNumber ?? existingUser.PhoneNumber;
            existingUser.Address = updateUserRequest.Address ?? existingUser.Address;

            // Không cập nhật các trường khác như roleId, skinTypeId, password, point, createAt, hoặc UserId

            await _context.SaveChangesAsync();

            return existingUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID {UserId}: {ErrorMessage}", userId, ex.Message);
            throw;
        }
    }

    // Delete user
    public async Task<bool> DeleteUserAsync(string userId)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", userId);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID {UserId}: {ErrorMessage}", userId, ex.Message);
            throw;
        }
    }
}