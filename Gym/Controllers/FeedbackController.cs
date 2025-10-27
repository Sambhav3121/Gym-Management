using Gym.DTO;
using Gym.Infrastructure;
using Gym.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Gym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;
        public FeedbackController(IFeedbackService service) => _service = service;

        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Submit([FromBody] SubmitFeedbackDto dto)
        {
            var userId = User.GetUserId();
            var result = await _service.SubmitAsync(userId, dto);
            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MyFeedback()
        {
            var userId = User.GetUserId();
            return Ok(await _service.GetMyFeedbackAsync(userId));
        }

        [HttpGet("trainer/{trainerId:guid}")]
        [Authorize(Roles = "Trainer,Admin")]
        public async Task<IActionResult> GetTrainerFeedback(Guid trainerId)
        {
            return Ok(await _service.GetTrainerFeedbackAsync(trainerId));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
          return ok
          ? Ok(new { message = "Feedback has been deleted successfully" })
          : NotFound(new { message = "Feedback not found" });

        }
    }
}
