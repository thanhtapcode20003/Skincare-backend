using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkinCare_Data;
using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Register;

public class AuthService
{
    private readonly SkinCare_DBContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(SkinCare_DBContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<(string Token, string RefreshToken)> LoginAsync(string email, string password)
    {
        // Giả sử bạn có một User table để kiểm tra thông tin đăng nhập
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || user.Password != HashPassword(password)) return (null, null);

        // Tạo Access Token
        string token = GenerateJwtToken(user);

        // Tạo Refresh Token
        string refreshToken = GenerateRefreshToken();

        // Lưu Refresh Token vào database
        var newRefreshToken = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.UserId.ToString(), // Chuyển User ID về string nếu cần
            ExpiryDate = DateTime.UtcNow.AddDays(7) // Refresh token có hiệu lực trong 7 ngày
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        return (token, refreshToken);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserId", user.UserId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // Token hết hạn sau 1 giờ
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<bool> RegisterUser(RegisterRequest request)
    {
        // Kiểm tra email đã tồn tại chưa
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
        {
            return false; // Email đã tồn tại
        }

        // Lấy RoleId từ RoleName
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.RoleName);
        if (role == null)
        {
            return false; // Role không hợp lệ
        }

        // Đếm số lượng user với cùng RoleId
        int userCount = await _context.Users.CountAsync(u => u.RoleId == role.RoleId);

        string userIdPrefix = role.RoleId switch
        {
            1 => "C",  // Customer
            2 => "S",  // Staff
            3 => "M",  // Manager
           
        };

        string newUserId = $"{userIdPrefix}{(userCount + 1):D3}"; // Ví dụ: C001, S002, M003


        // Tạo User mới
        var newUser = new User
        {
            UserId = newUserId,
            UserName = request.UserName,
            Email = request.Email,
            Password = HashPassword(request.Password), // Hash mật khẩu
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            RoleId = role.RoleId, // Gán RoleId từ RoleName
            SkinTypeId = null,
            CreateAt = DateTime.UtcNow
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return true;
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
