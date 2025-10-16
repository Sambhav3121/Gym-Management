using System;
using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        // Roles: Member, Trainer, Admin
        [Required]
        public string Role { get; set; } = "Member";

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }

        // ✅ Approval status
        public bool IsApproved { get; set; } = true;
    }
}
