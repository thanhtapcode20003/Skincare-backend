using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.User;
using System.Threading.Tasks;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(UserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

 
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        try
        {
            _logger.LogInformation("Fetching all users");

            var users = await _userService.GetAllUsersAsync();
            // Loại bỏ password khỏi response để bảo mật
            foreach (var user in users)
            {
                user.Password = null;
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching users: {ErrorMessage}", ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

   
    [HttpGet("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        try
        {
            _logger.LogInformation("Fetching user with ID: {UserId}", id);

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                return NotFound(new { message = "User not found" });
            }

            // Loại bỏ password khỏi response để bảo mật
            user.Password = null;
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user with ID {UserId}: {ErrorMessage}", id, ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        try
        {
            _logger.LogInformation("Creating new user with email: {Email}", user.Email);

            // Không cần gửi password qua body, chỉ nhận từ client nếu cần đăng ký
            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Password is required" });
            }

            var createdUser = await _userService.CreateUserAsync(user);
            // Loại bỏ password khỏi response để bảo mật
            createdUser.Password = null;
            return Ok(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user with email {Email}: {ErrorMessage}", user.Email, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    
    [HttpPut("edit/{id}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserRequest updateUserRequest)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);

            // Không kiểm tra UserId từ DTO, vì UserId không được gửi trong body
            var updatedUser = await _userService.UpdateUserAsync(id, updateUserRequest);
            // Loại bỏ password khỏi response để bảo mật
            updatedUser.Password = null;
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID {UserId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

   
    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);

            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID {UserId}: {ErrorMessage}", id, ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }
}