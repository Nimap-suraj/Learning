using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Models;

namespace TaskEcommerce.Context
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { } 
        
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }

        
    }
}
