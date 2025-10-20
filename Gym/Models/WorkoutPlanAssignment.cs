using System;

namespace Gym.Models
{
    public class WorkoutPlanAssignment
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string PlanName { get; set; } = "General";
        public bool IsActive { get; set; } = true;
        public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
