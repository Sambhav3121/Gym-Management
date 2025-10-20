using Gym.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ✅ Existing tables
        public DbSet<User> Users { get; set; }
        public DbSet<MembershipPlan> MembershipPlans { get; set; }
        public DbSet<UserMembership> UserMemberships { get; set; }

        // ✅ New tables we added
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<ClassSession> ClassSessions { get; set; }
        public DbSet<WorkoutPlanAssignment> WorkoutPlanAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Cascade delete: deleting a MembershipPlan deletes related UserMemberships
            modelBuilder.Entity<UserMembership>()
                .HasOne(um => um.MembershipPlan)
                .WithMany()
                .HasForeignKey(um => um.MembershipPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Cascade delete: deleting a User deletes their Memberships
            modelBuilder.Entity<UserMembership>()
                .HasOne(um => um.User)
                .WithMany()
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Configure Attendance (1 User -> Many Attendance records)
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.UserId, a.CheckInTimeUtc }); // for faster daily lookup

            // ✅ Configure class sessions (optional advanced rules later)
            modelBuilder.Entity<ClassSession>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(100);

            // ✅ Configure Workout plans (basic setup, extend later if needed)
            modelBuilder.Entity<WorkoutPlanAssignment>()
                .HasIndex(w => w.UserId);
        }
    }
}
