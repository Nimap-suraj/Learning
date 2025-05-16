using Microsoft.EntityFrameworkCore;

namespace TestCreate.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<Customer> Customers { get; set; }
    }
}
