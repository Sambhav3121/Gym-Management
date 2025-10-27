using Gym.DTO;
using Gym.Services;
using Gym.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserMembershipController : ControllerBase
    {
        private readonly IMembershipService _membership;
        private readonly IAttendanceService _attendance;
        private readonly IClassService _classes;

        public UserMembershipController(
            IMembershipService membership,
            IAttendanceService attendance,
            IClassService classes)
        {
            _membership = membership;
            _attendance = attendance;
            _classes = classes;
        }

        // ✅ MEMBERSHIP
        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase([FromBody] PurchaseMembershipDto dto)
        {
            var userId = User.GetUserId();
            var result = await _membership.PurchaseAsync(userId, dto.MembershipPlanId);
            return Ok(result);
        }

        [HttpPost("renew")]
        public async Task<IActionResult> Renew([FromBody] PurchaseMembershipDto dto)
        {
            var userId = User.GetUserId();
            var result = await _membership.RenewAsync(userId, dto.MembershipPlanId);
            return Ok(result);
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent()
        {
            var userId = User.GetUserId();
            var result = await _membership.GetCurrentAsync(userId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = User.GetUserId();
            var result = await _membership.GetHistoryAsync(userId);
            return Ok(result);
        }

        // ✅ ATTENDANCE
        [HttpPost("attendance/mark")]
        public async Task<IActionResult> MarkAttendance()
        {
            var userId = User.GetUserId();
            var success = await _attendance.MarkCheckInAsync(userId);

            return Ok(new
            {
                success,
                message = success ? "Checked in successfully" : "Already checked in today"
            });
        }

        [HttpGet("attendance/summary")]
        public async Task<IActionResult> AttendanceSummary()
        {
            var userId = User.GetUserId();
            var dto = await _attendance.GetSummaryAsync(userId);
            return Ok(dto);
        }

        [HttpGet("attendance/recent")]
        public async Task<IActionResult> RecentAttendance([FromQuery] int take = 5)
        {
            var userId = User.GetUserId();
            var list = await _attendance.GetRecentAsync(userId, take);
            return Ok(list);
        }

        // ✅ CLASSES
        [HttpGet("classes/count")]
        [AllowAnonymous] // change to [Authorize] if needed
        public async Task<IActionResult> GetAvailableClasses()
        {
            var count = await _classes.CountAvailableAsync();
            return Ok(new CountDto { Count = count });
        }

        // ✅ ADMIN MEMBERSHIP QUERIES
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMemberships()
        {
            return Ok(await _membership.GetAllAsync());
        }

        [HttpGet("{userId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var result = await _membership.GetByUserAsync(userId);
            return Ok(result);
        }
    }
}
