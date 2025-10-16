using System;

namespace Gym.DTO
{
    public class RegisterUserDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; } = "Member";
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsApproved { get; set; }   // ✅ Added for dashboard display
    }

    public class UpdateProfileDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
