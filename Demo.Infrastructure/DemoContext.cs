using Demo.Core;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure
{
    public class DemoContext : DbContext
    {
        public DbSet<Facility> Facilities { get; set; }

        public DemoContext(DbContextOptions<DemoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Facility>().HasData(
                new Facility(10000, "KNH"),
                new Facility(10001, "AKUH")
            );
        }
    }
}