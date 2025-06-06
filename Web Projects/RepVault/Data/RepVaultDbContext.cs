using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepVault.Models;



namespace RepVault.Data
{
    public class RepVaultDbContext : IdentityDbContext<ApplicationUser>
    {
        public RepVaultDbContext(DbContextOptions<RepVaultDbContext> options)
            : base(options)
        {
        }

        public DbSet<RepVaultWorkout> RepVaultWorkouts { get; set; }
        public DbSet<RepVaultExercise> RepVaultExercises { get; set; } // optional: for fixed list
        public DbSet<RepVaultGoal> RepVaultGoals { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
