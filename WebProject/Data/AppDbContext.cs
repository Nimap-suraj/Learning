using Microsoft.EntityFrameworkCore;
using WebProject.Data;

namespace WebApiProjectWithDto.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> categories { get; set; }
    }
}
