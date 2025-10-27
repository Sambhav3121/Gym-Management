using Gym.Data;
using Gym.DTO;
using Gym.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationDbContext _db;
        public FeedbackService(ApplicationDbContext db) => _db = db;

        public async Task<FeedbackResponseDto> SubmitAsync(Guid memberId, SubmitFeedbackDto dto)
        {
            var feedback = new Feedback
            {
                MemberId = memberId,
                TrainerId = dto.TrainerId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            _db.Feedbacks.Add(feedback);
            await _db.SaveChangesAsync();

            return Map(feedback);
        }

        public async Task<List<FeedbackResponseDto>> GetMyFeedbackAsync(Guid memberId)
        {
            return await _db.Feedbacks
                .Where(f => f.MemberId == memberId)
                .OrderByDescending(f => f.CreatedAtUtc)
                .Select(f => Map(f))
                .ToListAsync();
        }

        public async Task<List<FeedbackResponseDto>> GetTrainerFeedbackAsync(Guid trainerId)
        {
            return await _db.Feedbacks
                .Where(f => f.TrainerId == trainerId)
                .OrderByDescending(f => f.CreatedAtUtc)
                .Select(f => Map(f))
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var feedback = await _db.Feedbacks.FindAsync(id);
            if (feedback == null) return false;

            _db.Feedbacks.Remove(feedback);
            await _db.SaveChangesAsync();
            return true;
        }

        private static FeedbackResponseDto Map(Feedback f) => new()
        {
            Id = f.Id,
            Rating = f.Rating,
            Comment = f.Comment,
            MemberId = f.MemberId,
            TrainerId = f.TrainerId,
            CreatedAtUtc = f.CreatedAtUtc
        };
    }
}
