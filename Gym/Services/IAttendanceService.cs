using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gym.DTO;

namespace Gym.Services
{
    public interface IAttendanceService
    {
        Task<bool> MarkCheckInAsync(Guid userId);
        Task<AttendanceSummaryDto> GetSummaryAsync(Guid userId, int recent = 5);
        Task<IReadOnlyList<AttendanceItemDto>> GetRecentAsync(Guid userId, int take = 5);
    }
}
