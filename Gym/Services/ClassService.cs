using System;
using System.Linq;
using System.Threading.Tasks;
using Gym.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Services
{
    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext _db;
        public ClassService(ApplicationDbContext db) { _db = db; }

        public async Task<int> CountAvailableAsync()
        {
            var now = DateTime.UtcNow;
            return await _db.ClassSessions
                .Where(c => c.IsActive && c.StartTimeUtc >= now)
                .CountAsync();
        }
    }
}
