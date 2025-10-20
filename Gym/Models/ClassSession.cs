using System;

namespace Gym.Models
{
    public class ClassSession
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime StartTimeUtc { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
