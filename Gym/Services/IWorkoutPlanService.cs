using Gym.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gym.Services
{
    public interface IWorkoutPlanService
    {
        Task<WorkoutPlanDetailDto> CreateAsync(CreateWorkoutPlanDto dto, Guid creatorUserId);
        Task<WorkoutPlanDetailDto?> UpdateAsync(int id, UpdateWorkoutPlanDto dto);
        Task<bool> DeleteAsync(int id);
        Task<(IReadOnlyList<WorkoutPlanListItemDto> Items, int Total)> GetAllAsync(int page = 1, int pageSize = 20);
        Task<WorkoutPlanDetailDto?> GetByIdAsync(int id);

        Task<WorkoutExerciseDto> AddExerciseAsync(int planId, CreateWorkoutExerciseDto dto);
        Task<WorkoutExerciseDto?> UpdateExerciseAsync(int planId, int exerciseId, UpdateWorkoutExerciseDto dto);
        Task<bool> DeleteExerciseAsync(int planId, int exerciseId);
        Task<IReadOnlyList<WorkoutExerciseDto>> GetExercisesAsync(int planId);
    }
}
