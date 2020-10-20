using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    // We add this to our Startup class so we can inject DataContext into other parts of our application.
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }   // Create table called Users
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<AppUserAdvisor> Advised { get; set; }

        // Give entity configuration
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);  // Needed to avoid errors on migrations

            builder.Entity<UserLike>()
                .HasKey(k => new {k.SourceUserId, k.LikedUserId});  // Creates the primary key for the UserLike table

            // Define the relationship a:
            //      SourceUser can like many other users (l.LikedUsers)
            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);      // If we delete a user we delete the related entities
                // Use: .OnDelete(DeleteBehavior.NoAction); if using SQL server

            // Other side of relationship
            //      A LikedUser can have many LikedByUsers
            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);      // If we delete a user we delete the related entities


            builder.Entity<AppUserAdvisor>()
                .HasKey(k => new {k.SourceUserId, k.AdvisedUserId});  // Creates the primary key for the UserLike table

            // Define the relationship a:
            //      SourceUser can advise many other users (l.AdvisedUsers)
            builder.Entity<AppUserAdvisor>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.AdvisedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);      // If we delete a user we delete the related entities
                // Use: .OnDelete(DeleteBehavior.NoAction); if using SQL server

            // Other side of relationship
            //      An AdvisedUser can have many AdvisedByUsers
            //  Layman terms: A AdvisedUser can have many advisors on their AdvisedByUsers List
            builder.Entity<AppUserAdvisor>()
                .HasOne(s => s.AdvisedUser)
                .WithMany(l => l.AdvisedByUsers)
                .HasForeignKey(s => s.AdvisedUserId)
                .OnDelete(DeleteBehavior.Cascade);      // If we delete a user we delete the related entities

        }
        
    }
}