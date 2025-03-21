﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.Login;
using SkinCare_Data.DTO.Register;
using SkinCare_Data.IRepositories;
using SkinCare_Service.IService;
using BCrypt.Net;
using System.Threading.Tasks;

namespace SkinCare_Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        // Login
        public async Task<(string Token, string RefreshToken)> LoginAsync(string email, string password)
        {
            var user = await _repository.GetUserByEmailAsync(email);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                return (null, null);
            }

            // Access Token
            string token = GenerateJwtToken(user);

            // Refresh Token
            string refreshToken = GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.UserId.ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            await _repository.AddRefreshTokenAsync(newRefreshToken);
            await _repository.SaveChangesAsync();

            return (token, refreshToken);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fj8ui4h9874j5439utyh498hn9gh49g8y440983hng9843h983j8933902nmxoia9a10m3091mamaocp92k3mfpfdk239o2"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserName", user.UserName),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "Unknown"),
                new Claim("UserId", user.UserId),
                new Claim("PhoneNumber", user.PhoneNumber ?? ""),
                new Claim("Address", user.Address ?? ""),
                new Claim("CreateAt", user.CreateAt.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();
            return refreshToken;
        }

        public async Task<bool> RegisterUser(RegisterRequest request)
        {
            if (await _repository.UserExistsAsync(request.Email))
            {
                return false; 
            }

            var role = await _repository.GetRoleByNameAsync(request.RoleName);
            if (role == null)
            {
                return false; 
            }

            int userCount = await _repository.GetUserCountByRoleAsync(role.RoleId);

            string userIdPrefix = role.RoleId switch
            {
                1 => "C",  // Customer
                2 => "S",  // Staff
                3 => "M",  // Manager
                _ => throw new Exception("Invalid RoleId")
            };

            string newUserId = $"{userIdPrefix}{(userCount + 1):D3}"; 

            
            var newUser = new User
            {
                UserId = newUserId,
                UserName = request.UserName,
                Email = request.Email,
                Password = HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                RoleId = role.RoleId,
                SkinTypeId = null,
                CreateAt = DateTime.UtcNow
            };

            await _repository.AddUserAsync(newUser);
            await _repository.SaveChangesAsync();

            return true;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}