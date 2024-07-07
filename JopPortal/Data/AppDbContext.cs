using Microsoft.EntityFrameworkCore;
using JopPortal.Models;
using jobPortal.Models;

namespace JopPortal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplyJob>()
                .HasKey(e => new { e.UserId, e.JobId });
        }
        public DbSet<User> user { get; set; }
        public DbSet<Company> company { get; set; }
        public DbSet<Job> jop { get; set; }
        public DbSet<ApplyJob> applyJop  { get; set; }

    }
}
