using Gym.Data;
using Gym.DTO;
using Gym.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly ApplicationDbContext _context;

        public MembershipService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====== PLANS ======
        public async Task<List<MembershipPlanDto>> GetAllPlansAsync()
        {
            return await _context.MembershipPlans
                .Select(p => new MembershipPlanDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    DurationInDays = p.DurationInDays
                }).ToListAsync();
        }

        public async Task<MembershipPlanDto?> GetPlanByIdAsync(int id)
        {
            var plan = await _context.MembershipPlans.FindAsync(id);
            return plan == null ? null : new MembershipPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                DurationInDays = plan.DurationInDays
            };
        }

        public async Task<MembershipPlanDto> CreatePlanAsync(CreateMembershipPlanDto dto)
        {
            var plan = new MembershipPlan
            {
                Name = dto.Name,
                Price = dto.Price,
                DurationInDays = dto.DurationInDays
            };
            _context.MembershipPlans.Add(plan);
            await _context.SaveChangesAsync();

            return new MembershipPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                DurationInDays = plan.DurationInDays
            };
        }

        public async Task<MembershipPlanDto?> UpdatePlanAsync(int id, CreateMembershipPlanDto dto)
        {
            var plan = await _context.MembershipPlans.FindAsync(id);
            if (plan == null) return null;

            plan.Name = dto.Name;
            plan.Price = dto.Price;
            plan.DurationInDays = dto.DurationInDays;
            await _context.SaveChangesAsync();

            return new MembershipPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                DurationInDays = plan.DurationInDays
            };
        }

        public async Task<bool> DeletePlanAsync(int id)
        {
            var plan = await _context.MembershipPlans.FindAsync(id);
            if (plan == null) return false;

            _context.MembershipPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }

        // ====== USER MEMBERSHIPS ======
        public async Task<UserMembershipDto> PurchaseAsync(Guid userId, int planId)
        {
            var plan = await _context.MembershipPlans.FindAsync(planId)
                ?? throw new Exception("Plan not found.");

            var startDate = DateTime.UtcNow;
            var expiryDate = startDate.AddDays(plan.DurationInDays);

            var membership = new UserMembership
            {
                UserId = userId,
                MembershipPlanId = planId,
                StartDate = startDate,
                ExpiryDate = expiryDate,
                IsActive = true
            };

            _context.UserMemberships.Add(membership);
            await _context.SaveChangesAsync();

            return new UserMembershipDto
            {
                Id = membership.Id,
                PlanName = plan.Name,
                StartDate = startDate,
                ExpiryDate = expiryDate,
                IsActive = true
            };
        }

        public async Task<UserMembershipDto> RenewAsync(Guid userId, int planId)
        {
            var plan = await _context.MembershipPlans.FindAsync(planId)
                ?? throw new Exception("Plan not found.");

            var lastMembership = await _context.UserMemberships
                .Where(u => u.UserId == userId && u.IsActive)
                .OrderByDescending(u => u.ExpiryDate)
                .FirstOrDefaultAsync();

            DateTime startDate = (lastMembership != null && lastMembership.ExpiryDate > DateTime.UtcNow)
                ? lastMembership.ExpiryDate
                : DateTime.UtcNow;

            var expiryDate = startDate.AddDays(plan.DurationInDays);

            var membership = new UserMembership
            {
                UserId = userId,
                MembershipPlanId = planId,
                StartDate = startDate,
                ExpiryDate = expiryDate,
                IsActive = true
            };

            _context.UserMemberships.Add(membership);
            await _context.SaveChangesAsync();

            return new UserMembershipDto
            {
                Id = membership.Id,
                PlanName = plan.Name,
                StartDate = startDate,
                ExpiryDate = expiryDate,
                IsActive = true
            };
        }

        public async Task<UserMembershipDto?> GetCurrentAsync(Guid userId)
        {
            var current = await _context.UserMemberships
                .Include(u => u.MembershipPlan)
                .Where(u => u.UserId == userId && u.IsActive && u.ExpiryDate > DateTime.UtcNow && u.MembershipPlan != null)
                .OrderByDescending(u => u.ExpiryDate)
                .FirstOrDefaultAsync();

            return current == null ? null : new UserMembershipDto
            {
                Id = current.Id,
                PlanName = current.MembershipPlan.Name,
                StartDate = current.StartDate,
                ExpiryDate = current.ExpiryDate,
                IsActive = current.IsActive
            };
        }

        public async Task<List<UserMembershipDto>> GetHistoryAsync(Guid userId)
        {
            return await _context.UserMemberships
                .Include(u => u.MembershipPlan)
                .Where(u => u.UserId == userId && u.MembershipPlan != null)
                .OrderByDescending(u => u.StartDate)
                .Select(u => new UserMembershipDto
                {
                    Id = u.Id,
                    PlanName = u.MembershipPlan.Name,
                    StartDate = u.StartDate,
                    ExpiryDate = u.ExpiryDate,
                    IsActive = u.IsActive
                }).ToListAsync();
        }

        public async Task<List<UserMembershipDto>> GetAllAsync()
        {
            return await _context.UserMemberships
                .Include(u => u.MembershipPlan)
                .Where(u => u.MembershipPlan != null)
                .Select(u => new UserMembershipDto
                {
                    Id = u.Id,
                    PlanName = u.MembershipPlan.Name,
                    StartDate = u.StartDate,
                    ExpiryDate = u.ExpiryDate,
                    IsActive = u.IsActive
                }).ToListAsync();
        }

        public async Task<List<UserMembershipDto>> GetByUserAsync(Guid userId)
        {
            return await _context.UserMemberships
                .Include(u => u.MembershipPlan)
                .Where(u => u.UserId == userId && u.MembershipPlan != null)
                .Select(u => new UserMembershipDto
                {
                    Id = u.Id,
                    PlanName = u.MembershipPlan.Name,
                    StartDate = u.StartDate,
                    ExpiryDate = u.ExpiryDate,
                    IsActive = u.IsActive
                }).ToListAsync();
        }
    }
}
