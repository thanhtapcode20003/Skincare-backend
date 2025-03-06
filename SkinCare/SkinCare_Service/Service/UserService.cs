using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.User;
using SkinCare_Data.IRepositories;
using SkinCare_Service.IService;
using System;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SkinCare_Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repository, ILogger<UserService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Get all users
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users");

                var users = await _repository.GetAllAsync();
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

                var user = await _repository.GetByIdAsync(userId);
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

                if (await _repository.UserExistsAsync(user.Email))
                {
                    throw new Exception("Email already exists!");
                }

                var role = await _repository.FindRoleByIdAsync(user.RoleId);
                if (role == null)
                {
                    throw new Exception("Role not found!");
                }

                int userCount = await _repository.GetUserCountByRoleAsync(role.RoleId);
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

                await _repository.AddAsync(user);
                await _repository.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with email {Email}: {ErrorMessage}", user.Email, ex.Message);
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(string userId, UpdateUserRequest updateUserRequest)
        {
            try
            {
                _logger.LogInformation("Updating user with ID: {UserId}", userId);

                var existingUser = await _repository.GetByIdAsync(userId);
                if (existingUser == null)
                {
                    throw new Exception("User not found!");
                }

                existingUser.UserName = updateUserRequest.UserName ?? existingUser.UserName;
                if (updateUserRequest.Email != null && updateUserRequest.Email != existingUser.Email)
                {
                    if (await _repository.UserExistsAsync(updateUserRequest.Email) && (await _repository.GetByIdAsync(userId)).Email != updateUserRequest.Email)
                    {
                        throw new Exception("Email already exists for another user!");
                    }
                    existingUser.Email = updateUserRequest.Email;
                }
                existingUser.PhoneNumber = updateUserRequest.PhoneNumber ?? existingUser.PhoneNumber;
                existingUser.Address = updateUserRequest.Address ?? existingUser.Address;


                await _repository.UpdateAsync(existingUser);
                await _repository.SaveChangesAsync();

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

                var user = await _repository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", userId);
                    return false;
                }

                await _repository.DeleteAsync(user);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}: {ErrorMessage}", userId, ex.Message);
                throw;
            }
        }
    }
}