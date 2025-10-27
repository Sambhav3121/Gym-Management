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

        public DbSet<User> Users { get; set; }
        public DbSet<MembershipPlan> MembershipPlans { get; set; }
        public DbSet<UserMembership> UserMemberships { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<ClassSession> ClassSessions { get; set; }
       
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<UserMembership>()
                .HasOne(um => um.MembershipPlan)
                .WithMany()
                .HasForeignKey(um => um.MembershipPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserMembership>()
                .HasOne(um => um.User)
                .WithMany()
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.UserId, a.CheckInTimeUtc });

            
            modelBuilder.Entity<ClassSession>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(100);
            
             modelBuilder.Entity<WorkoutPlan>()
                .HasMany(p => p.Exercises)
                .WithOne(e => e.WorkoutPlan)
                .HasForeignKey(e => e.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
