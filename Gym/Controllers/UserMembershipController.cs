using Gym.DTO;
using Gym.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserMembershipController : ControllerBase
    {
        private readonly IMembershipService _service;

        public UserMembershipController(IMembershipService service)
        {
            _service = service;
        }

        [HttpPost("purchase")]
        [Authorize]
        public async Task<IActionResult> Purchase([FromBody] PurchaseMembershipDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var result = await _service.PurchaseAsync(userId, dto.MembershipPlanId);
            return Ok(result);
        }

        [HttpPost("renew")]
        [Authorize]
        public async Task<IActionResult> Renew([FromBody] PurchaseMembershipDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var result = await _service.RenewAsync(userId, dto.MembershipPlanId);
            return Ok(result);
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrent()
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var result = await _service.GetCurrentAsync(userId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> GetHistory()
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var result = await _service.GetHistoryAsync(userId);
            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{userId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var result = await _service.GetByUserAsync(userId);
            return Ok(result);
        }
    }
}
