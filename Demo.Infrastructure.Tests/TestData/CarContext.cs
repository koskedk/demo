using Demo.SharedKernel.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Tests.TestData
{
    public class CarContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public CarContext(DbContextOptions<CarContext> options) : base(options)
        {
        }
    }
}