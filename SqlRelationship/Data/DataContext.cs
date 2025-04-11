using Microsoft.EntityFrameworkCore;
using SqlRelationship.Entities;

namespace SqlRelationship.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Address> Address { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserCoupon> UserCoupon { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasData(new Address { Id = 1, StreetNumber = "Avenue 100Th" });
            modelBuilder.Entity<Coupon>().HasData(new Coupon { Id = 1, Code = "FIFO" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 1,Title = "Shoe",Price = 122,UserId = 1});
            modelBuilder.Entity<User>().HasData(new User { Id = 1,Name = "suraj shah",AddressId = 1 });
          //  modelBuilder.Entity<UserCoupon>().HasData(new UserCoupon { UserId = 1,CouponId = 1 });


            modelBuilder.Entity<User>()
                .HasMany(e => e.Coupons)
                .WithMany(e => e.Users)
                .UsingEntity<UserCoupon>();

            modelBuilder.Entity<UserCoupon>().HasData(new UserCoupon { UserId = 1,CouponId = 1 });



        }


    }
}
