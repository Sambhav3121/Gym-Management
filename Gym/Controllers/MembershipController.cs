using Gym.DTO;
using Gym.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipPlanController : ControllerBase
    {
        private readonly IMembershipService _service;

        public MembershipPlanController(IMembershipService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllPlansAsync());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var plan = await _service.GetPlanByIdAsync(id);
            return plan == null ? NotFound() : Ok(plan);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateMembershipPlanDto dto)
        {
            var plan = await _service.CreatePlanAsync(dto);
            return Ok(plan);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateMembershipPlanDto dto)
        {
            var plan = await _service.UpdatePlanAsync(id, dto);
            return plan == null ? NotFound() : Ok(plan);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeletePlanAsync(id);
            return ok ? Ok(new { message = "Deleted successfully." }) : NotFound();
        }
    }
}
