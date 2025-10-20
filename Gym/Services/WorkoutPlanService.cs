using System;
using System.Linq;
using System.Threading.Tasks;
using Gym.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Services
{
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly ApplicationDbContext _db;
        public WorkoutPlanService(ApplicationDbContext db) { _db = db; }

        public async Task<int> CountUserActiveAsync(Guid userId)
        {
            return await _db.WorkoutPlanAssignments
                .Where(w => w.UserId == userId && w.IsActive)
                .CountAsync();
        }
    }
}
