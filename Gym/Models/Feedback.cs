using System;
using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public Guid MemberId { get; set; }   
        public Guid TrainerId { get; set; }  

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
