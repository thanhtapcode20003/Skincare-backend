﻿using Microsoft.AspNetCore.Mvc;
using SkinCare_Data.DTO.Login;
using SkinCare_Data.DTO.Register;
using SkinCare_Service.IService; 
using System.Threading.Tasks;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService; 

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterUser(request);
        if (!result)
        {
            return BadRequest(new { message = "Email is exist!" });
        }

        return Ok(new { message = "Success!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (token, refreshToken) = await _authService.LoginAsync(request.Email, request.Password);

        if (token == null) return Unauthorized("Invalid username or password.");

        return Ok(new { Token = token, RefreshToken = refreshToken });
    }
}