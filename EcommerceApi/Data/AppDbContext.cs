using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models;

namespace EcommerceApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // "Hey, when someone creates an AppDbContext, give it the connection info (options),
        // //and I'll pass it to the EF Core engine so it knows how to talk to the database."
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        
    }
}
