using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class MembershipPlan
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!; // Monthly, Quarterly, Yearly

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int DurationInDays { get; set; } // 30, 90, 365
    }
}
