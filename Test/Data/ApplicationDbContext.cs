using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Test.Model;

namespace Test.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
       
        public DbSet<Product> products {  get; set; }
        public DbSet<Category> Category {  get; set; }
        public DbSet<Customer> Customers {  get; set; }
        public DbSet<Order> Orders {  get; set; }
    }
}
 