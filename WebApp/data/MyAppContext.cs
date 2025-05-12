using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using WebApp.Models;

namespace WebApp.data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ItemClient>().HasKey(
                ic => new
                {
                    ic.ClientId,
                    ic.ItemId
                });

            modelBuilder.Entity<ItemClient>().HasOne( i => i.Item).WithMany(c => c.ItemClients).HasForeignKey(i => i.ItemId);
            modelBuilder.Entity<ItemClient>().HasOne( i => i.Client).WithMany(c => c.ItemClients).HasForeignKey(i => i.ClientId);
            modelBuilder.Entity<Item>().HasData(
               new Item { Id = 8, Name = "Watch", Price = 140, SerialNumberId=10 }
               );
            modelBuilder.Entity<SerialNumber>().HasData(
                new SerialNumber { Id = 10, Name = "Wtc1", ItemId = 8}
                );

            modelBuilder.Entity<Category>().HasData
                (
                new Category { Id = 1,Name="Electronics" },
                new Category { Id = 2,Name="Books" },
                new Category { Id = 3,Name="Fashions" },
                new Category { Id = 4,Name="Home Application" },
                new Category { Id = 5,Name="Jwellary" },
                new Category { Id = 6, Name = "Deo Drant" }
                );
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Item> Items { get; set; } 
        public DbSet<SerialNumber> SerialNumbers { get; set; } 
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Client> Clients { get; set; } 
        public DbSet<ItemClient> ItemClients { get; set; } 
        public DbSet<UserAccount> userAccounts { get; set; } 
    }
}
