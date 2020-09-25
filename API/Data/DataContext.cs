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
    }
}