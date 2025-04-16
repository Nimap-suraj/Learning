using Microsoft.EntityFrameworkCore;
using WebServices.Model;

namespace WebServices.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options):base(options) { }
        public DbSet<User> Users { get; set; }
        //public DbSet<CategoryDATA> categories { get; set; }
        public DbSet<Category> categories { get; set; }

    }
}
