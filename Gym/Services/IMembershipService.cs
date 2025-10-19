using Gym.DTO;

namespace Gym.Services
{
    public interface IMembershipService
    {
        // Membership plans
        Task<List<MembershipPlanDto>> GetAllPlansAsync();
        Task<MembershipPlanDto?> GetPlanByIdAsync(int id);
        Task<MembershipPlanDto> CreatePlanAsync(CreateMembershipPlanDto dto);
        Task<MembershipPlanDto?> UpdatePlanAsync(int id, CreateMembershipPlanDto dto);
        Task<bool> DeletePlanAsync(int id);

        // User memberships
        Task<UserMembershipDto> PurchaseAsync(Guid userId, int planId);
        Task<UserMembershipDto> RenewAsync(Guid userId, int planId);
        Task<UserMembershipDto?> GetCurrentAsync(Guid userId);
        Task<List<UserMembershipDto>> GetHistoryAsync(Guid userId);
        Task<List<UserMembershipDto>> GetAllAsync(); // admin
        Task<List<UserMembershipDto>> GetByUserAsync(Guid userId); // admin
    }
}
