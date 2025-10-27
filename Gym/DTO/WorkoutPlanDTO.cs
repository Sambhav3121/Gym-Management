using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gym.DTO
{
    public class CreateWorkoutPlanDto
    {
        [Required, MaxLength(120)]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPublished { get; set; } = true;
    }

    public class UpdateWorkoutPlanDto
    {
        [Required, MaxLength(120)]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPublished { get; set; } = true;
    }

    public class WorkoutPlanListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class WorkoutExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Note { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int OrderIndex { get; set; }
    }

    public class WorkoutPlanDetailDto : WorkoutPlanListItemDto
    {
        public Guid CreatedByUserId { get; set; }
        public List<WorkoutExerciseDto> Exercises { get; set; } = new();
    }

    public class CreateWorkoutExerciseDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Note { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int OrderIndex { get; set; } = 1;
    }

    public class UpdateWorkoutExerciseDto : CreateWorkoutExerciseDto { }
}
