using System;
using System.Collections.Generic;

namespace Gym.DTO
{
    public class AttendanceSummaryDto
    {
        public int TotalVisits { get; set; }
        public List<AttendanceItemDto> Recent { get; set; } = new();
    }

    public class AttendanceItemDto
    {
        public DateTime CheckInTimeUtc { get; set; }
    }
}
