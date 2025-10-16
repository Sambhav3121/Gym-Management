using Gym.DTO;

namespace Gym.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> RegisterAsync(RegisterUserDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<UserProfileDto?> GetProfileAsync(string email);
        Task<UserProfileDto?> UpdateProfileAsync(string email, UpdateProfileDto dto);
        Task<List<UserProfileDto>> GetAllAsync();
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> ApproveUserAsync(Guid userId); // ✅ Admin approval
        Task SeedAdminAsync();
    }
}
