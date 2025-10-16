using Gym.DTO;
using Gym.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ✅ REGISTER (Public)
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                var result = await _userService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ LOGIN (Public)
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _userService.LoginAsync(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // ✅ LOGOUT (Authenticated)
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logged out successfully." });
        }

        // ✅ GET PROFILE (Authenticated)
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var result = await _userService.GetProfileAsync(email!);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // ✅ UPDATE PROFILE (Authenticated)
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var result = await _userService.UpdateProfileAsync(email!, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // ✅ GET ALL USERS (Admin or Trainer)
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // ✅ DELETE USER (Admin or Trainer)
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var ok = await _userService.DeleteAsync(id);
            return ok ? Ok(new { message = "User deleted." }) : NotFound();
        }

        // ✅ APPROVE TRAINER (Admin only)
        [HttpPut("approve/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveUser(Guid id)
        {
            var result = await _userService.ApproveUserAsync(id);
            if (!result)
                return NotFound(new { message = "User not found or already approved." });

            return Ok(new { message = "User approved successfully." });
        }
    }
}
