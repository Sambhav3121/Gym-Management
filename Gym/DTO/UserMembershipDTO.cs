namespace Gym.DTO
{
    public class PurchaseMembershipDto
    {
        public int MembershipPlanId { get; set; }
    }

    public class UserMembershipDto
    {
        public int Id { get; set; }
        public string PlanName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
