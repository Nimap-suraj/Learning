using Microsoft.EntityFrameworkCore;
using Task.Model;

namespace Task.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
      
        public DbSet<Department> Departments { get; set; }

    }
}
