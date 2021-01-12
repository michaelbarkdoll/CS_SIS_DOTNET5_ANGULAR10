using System;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data
{
    // We add this to our Startup class so we can inject DataContext into other parts of our application.
    //public class DataContext : DbContext


    // We inherit from IdentityDbContext and give it all of its types.
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // public DbSet<AppUser> Users { get; set; }   // Create table called Users     // Remove due to being provided by IdentityDbContext
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<AppUserAdvisor> Advised { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<PrintJob> PrintJobs { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<UserContainer> UserContainers { get; set; }

        // Give entity configuration
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);  // Needed to avoid errors on migrations

            // Configure relationship between AppUser and our UserRoles
            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            // Configure other side of the relationship between AppRole and our UserRoles
            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId });  // Creates the primary key for the UserLike table

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
                .HasKey(k => new { k.SourceUserId, k.AdvisedUserId });  // Creates the primary key for the UserLike table

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



            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);     // We don't want to remove the messages if one user deletes

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);     // We don't want to remove the messages if one user deletes
            
            builder.ApplyUtcDateTimeConverter();
        }

    }

    public static class UtcDateAnnotation
    {
        private const String IsUtcAnnotation = "IsUtc";
        private static readonly ValueConverter<DateTime, DateTime> UtcConverter =
          new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        private static readonly ValueConverter<DateTime?, DateTime?> UtcNullableConverter =
          new ValueConverter<DateTime?, DateTime?>(v => v, v => v == null ? v : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

        public static PropertyBuilder<TProperty> IsUtc<TProperty>(this PropertyBuilder<TProperty> builder, Boolean isUtc = true) =>
          builder.HasAnnotation(IsUtcAnnotation, isUtc);

        public static Boolean IsUtc(this IMutableProperty property) =>
          ((Boolean?)property.FindAnnotation(IsUtcAnnotation)?.Value) ?? true;

        /// <summary>
        /// Make sure this is called after configuring all your entities.
        /// </summary>
        public static void ApplyUtcDateTimeConverter(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (!property.IsUtc())
                    {
                        continue;
                    }

                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(UtcConverter);
                    }

                    if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(UtcNullableConverter);
                    }
                }
            }
        }
    }
}