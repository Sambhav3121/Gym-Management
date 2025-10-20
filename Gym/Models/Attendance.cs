using System;

namespace Gym.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CheckInTimeUtc { get; set; } = DateTime.UtcNow;
    }
}
