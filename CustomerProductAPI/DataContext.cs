using CustomerProductAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerProductAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(cp => cp.Products)
                .WithMany(cp => cp.Customers)
                .UsingEntity(cp => cp.ToTable("CustomerProduct"));
        }


    }
}
