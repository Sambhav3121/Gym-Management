namespace Gym.DTO
{
    public class CreateMembershipPlanDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
    }

    public class MembershipPlanDto : CreateMembershipPlanDto
    {
        public int Id { get; set; }
    }
}
