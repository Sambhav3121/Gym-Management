using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gym.Models
{
    public class WorkoutExercise
    {
        public int Id { get; set; }

        [ForeignKey(nameof(WorkoutPlan))]
        public int WorkoutPlanId { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; } = null!;

        [Required, MaxLength(120)]
        public string Name { get; set; } = null!;

        [MaxLength(300)]
        public string? Note { get; set; }

        [Range(1, 100)]
        public int Sets { get; set; }

        [Range(1, 1000)]
        public int Reps { get; set; }

        public int OrderIndex { get; set; } = 1;
    }
}
