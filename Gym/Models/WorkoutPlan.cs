using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class WorkoutPlan
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public Guid CreatedByUserId { get; set; }
        public bool IsPublished { get; set; } = true;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }

        public List<WorkoutExercise> Exercises { get; set; } = new();
    }
}
