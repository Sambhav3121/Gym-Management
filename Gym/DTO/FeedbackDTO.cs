using System;
using System.ComponentModel.DataAnnotations;

namespace Gym.DTO
{
    public class SubmitFeedbackDto
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public Guid TrainerId { get; set; }
    }

    public class FeedbackResponseDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public Guid MemberId { get; set; }
        public Guid TrainerId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
