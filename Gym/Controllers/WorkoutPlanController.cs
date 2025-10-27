using Gym.DTO;
using Gym.Services;
using Gym.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanService _service;
        public WorkoutPlanController(IWorkoutPlanService service) => _service = service;

        [HttpPost]
        [Authorize(Roles = "Trainer,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutPlanDto dto)
        {
            var userId = User.GetUserId();
            var result = await _service.CreateAsync(dto, userId);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Trainer,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkoutPlanDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Trainer,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok
              ? Ok(new { message = "Workout plan deleted successfully" })
               :  NotFound(new { message = "Workout plan not found" });

        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var (items, total) = await _service.GetAllAsync(page, pageSize);
            return Ok(new { total, page, pageSize, items });
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var plan = await _service.GetByIdAsync(id);
            return plan is null ? NotFound() : Ok(plan);
        }

        [HttpPost("{planId:int}/exercises")]
        [Authorize(Roles = "Trainer,Admin")]
        public async Task<IActionResult> AddExercise(int planId, [FromBody] CreateWorkoutExerciseDto dto)
        {
            var ex = await _service.AddExerciseAsync(planId, dto);
            return Ok(ex);
        }

        [HttpGet("{planId:int}/exercises")]
        [AllowAnonymous]
        public async Task<IActionResult> GetExercises(int planId)
        {
            var list = await _service.GetExercisesAsync(planId);
            return Ok(list);
        }

        [HttpPut("{planId:int}/exercises/{exerciseId:int}")]
        [Authorize(Roles = "Trainer,Admin")]
        public async Task<IActionResult> UpdateExercise(int planId, int exerciseId, [FromBody] UpdateWorkoutExerciseDto dto)
        {
            var ex = await _service.UpdateExerciseAsync(planId, exerciseId, dto);
            return ex is null ? NotFound() : Ok(ex);
        }

        [HttpDelete("{planId:int}/exercises/{exerciseId:int}")]
        [Authorize(Roles = "Trainer,Admin")]
        public async Task<IActionResult> DeleteExercise(int planId, int exerciseId)
        {
            var ok = await _service.DeleteExerciseAsync(planId, exerciseId);
            return ok ? NoContent() : NotFound();
        }
    }
}
