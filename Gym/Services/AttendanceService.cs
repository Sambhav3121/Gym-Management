using System;
using System.Linq;
using System.Threading.Tasks;
using Gym.Data;
using Gym.DTO;
using Gym.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _db;

        public AttendanceService(ApplicationDbContext db)
        {
            _db = db;
        }

        // B1: one check-in per day (UTC date)
        public async Task<bool> MarkCheckInAsync(Guid userId)
        {
            var today = DateTime.UtcNow.Date;

            var alreadyToday = await _db.Attendances
                .AnyAsync(a => a.UserId == userId && a.CheckInTimeUtc.Date == today);

            if (alreadyToday) return false;

            _db.Attendances.Add(new Attendance
            {
                UserId = userId,
                CheckInTimeUtc = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<AttendanceSummaryDto> GetSummaryAsync(Guid userId, int recent = 5)
        {
            var total = await _db.Attendances.CountAsync(a => a.UserId == userId);

            var recentItems = await _db.Attendances
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CheckInTimeUtc)
                .Take(recent)
                .Select(a => new AttendanceItemDto { CheckInTimeUtc = a.CheckInTimeUtc })
                .ToListAsync();

            return new AttendanceSummaryDto
            {
                TotalVisits = total,
                Recent = recentItems
            };
        }

        public async Task<IReadOnlyList<AttendanceItemDto>> GetRecentAsync(Guid userId, int take = 5)
        {
            var items = await _db.Attendances
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CheckInTimeUtc)
                .Take(take)
                .Select(a => new AttendanceItemDto { CheckInTimeUtc = a.CheckInTimeUtc })
                .ToListAsync();

            return items;
        }
    }
}
