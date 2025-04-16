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
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Category>()
            //    .HasMany(c => c.Products)
            //    .WithOne(p => p.Category)
            //    .HasForeignKey(p => p.CategoryId);
       //     modelBuilder.Entity<Order>()
       //.HasOne(o => o.User)
       //.WithMany(u => u.Orders)
       //.HasForeignKey(o => o.UserId)
       //.OnDelete(DeleteBehavior.Cascade); // 👈 Enables cascade delete
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.User.isDeleted);
            // In OnModelCreating()
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);

            modelBuilder.Entity<User>().HasQueryFilter(u => !u.isDeleted);
        }

    }
}
