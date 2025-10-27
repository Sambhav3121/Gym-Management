using Gym.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gym.Services
{
    public interface IFeedbackService
    {
        Task<FeedbackResponseDto> SubmitAsync(Guid memberId, SubmitFeedbackDto dto);
        Task<List<FeedbackResponseDto>> GetMyFeedbackAsync(Guid memberId);
        Task<List<FeedbackResponseDto>> GetTrainerFeedbackAsync(Guid trainerId);
        Task<bool> DeleteAsync(int id);
    }
}
