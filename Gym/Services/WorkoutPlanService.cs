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
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly ApplicationDbContext _db;
        public WorkoutPlanService(ApplicationDbContext db) => _db = db;

        public async Task<WorkoutPlanDetailDto> CreateAsync(CreateWorkoutPlanDto dto, Guid creatorUserId)
        {
            var plan = new WorkoutPlan
            {
                Title = dto.Title,
                Description = dto.Description,
                IsPublished = dto.IsPublished,
                CreatedByUserId = creatorUserId
            };

            _db.WorkoutPlans.Add(plan);
            await _db.SaveChangesAsync();
            return await GetByIdAsync(plan.Id) ?? throw new Exception("Could not fetch created plan");
        }

        public async Task<WorkoutPlanDetailDto?> UpdateAsync(int id, UpdateWorkoutPlanDto dto)
        {
            var plan = await _db.WorkoutPlans.FindAsync(id);
            if (plan == null) return null;

            plan.Title = dto.Title;
            plan.Description = dto.Description;
            plan.IsPublished = dto.IsPublished;
            plan.UpdatedAtUtc = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var plan = await _db.WorkoutPlans.FindAsync(id);
            if (plan == null) return false;

            _db.WorkoutPlans.Remove(plan);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<(IReadOnlyList<WorkoutPlanListItemDto> Items, int Total)> GetAllAsync(int page = 1, int pageSize = 20)
        {
            var query = _db.WorkoutPlans.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc);
            var total = await query.CountAsync();

            var items = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .Select(p => new WorkoutPlanListItemDto
                                   {
                                       Id = p.Id,
                                       Title = p.Title,
                                       Description = p.Description,
                                       CreatedAtUtc = p.CreatedAtUtc,
                                       IsPublished = p.IsPublished
                                   }).ToListAsync();

            return (items, total);
        }

        public async Task<WorkoutPlanDetailDto?> GetByIdAsync(int id)
        {
            return await _db.WorkoutPlans
                .Include(p => p.Exercises.OrderBy(x => x.OrderIndex))
                .Select(p => new WorkoutPlanDetailDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CreatedAtUtc = p.CreatedAtUtc,
                    IsPublished = p.IsPublished,
                    CreatedByUserId = p.CreatedByUserId,
                    Exercises = p.Exercises
                        .OrderBy(x => x.OrderIndex)
                        .Select(e => new WorkoutExerciseDto
                        {
                            Id = e.Id,
                            Name = e.Name,
                            Note = e.Note,
                            Sets = e.Sets,
                            Reps = e.Reps,
                            OrderIndex = e.OrderIndex
                        }).ToList()
                })
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<WorkoutExerciseDto> AddExerciseAsync(int planId, CreateWorkoutExerciseDto dto)
        {
            var exists = await _db.WorkoutPlans.AnyAsync(p => p.Id == planId);
            if (!exists) throw new KeyNotFoundException("Workout plan not found.");

            var ex = new WorkoutExercise
            {
                WorkoutPlanId = planId,
                Name = dto.Name,
                Note = dto.Note,
                Sets = dto.Sets,
                Reps = dto.Reps,
                OrderIndex = dto.OrderIndex
            };

            _db.WorkoutExercises.Add(ex);
            await _db.SaveChangesAsync();
            return new WorkoutExerciseDto
            {
                Id = ex.Id,
                Name = ex.Name,
                Note = ex.Note,
                Sets = ex.Sets,
                Reps = ex.Reps,
                OrderIndex = ex.OrderIndex
            };
        }

        public async Task<WorkoutExerciseDto?> UpdateExerciseAsync(int planId, int exerciseId, UpdateWorkoutExerciseDto dto)
        {
            var ex = await _db.WorkoutExercises.FirstOrDefaultAsync(e => e.Id == exerciseId && e.WorkoutPlanId == planId);
            if (ex == null) return null;

            ex.Name = dto.Name;
            ex.Note = dto.Note;
            ex.Sets = dto.Sets;
            ex.Reps = dto.Reps;
            ex.OrderIndex = dto.OrderIndex;

            await _db.SaveChangesAsync();
            return new WorkoutExerciseDto
            {
                Id = ex.Id,
                Name = ex.Name,
                Note = ex.Note,
                Sets = ex.Sets,
                Reps = ex.Reps,
                OrderIndex = ex.OrderIndex
            };
        }

        public async Task<bool> DeleteExerciseAsync(int planId, int exerciseId)
        {
            var ex = await _db.WorkoutExercises.FirstOrDefaultAsync(e => e.Id == exerciseId && e.WorkoutPlanId == planId);
            if (ex == null) return false;

            _db.WorkoutExercises.Remove(ex);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<WorkoutExerciseDto>> GetExercisesAsync(int planId)
        {
            return await _db.WorkoutExercises
                .Where(e => e.WorkoutPlanId == planId)
                .OrderBy(e => e.OrderIndex)
                .Select(e => new WorkoutExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Note = e.Note,
                    Sets = e.Sets,
                    Reps = e.Reps,
                    OrderIndex = e.OrderIndex
                })
                .ToListAsync();
        }
    }
}
