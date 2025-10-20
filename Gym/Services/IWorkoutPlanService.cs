using System;
using System.Threading.Tasks;

namespace Gym.Services
{
    public interface IWorkoutPlanService
    {
        Task<int> CountUserActiveAsync(Guid userId);
    }
}
