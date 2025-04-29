using Microsoft.EntityFrameworkCore;
using ProdBase.Domain.Entities;

namespace ProdBase.Infrastructure.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserProfile entity
            modelBuilder.Entity<UserProfile>()
                .HasIndex(u => u.FirebaseUID)
                .IsUnique();
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is UserProfile && (
                    e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var userProfile = (UserProfile)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    userProfile.CreatedAt = DateTime.UtcNow;
                }

                userProfile.UpdatedAt = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is UserProfile && (
                    e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var userProfile = (UserProfile)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    userProfile.CreatedAt = DateTime.UtcNow;
                }

                userProfile.UpdatedAt = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
