using api_EXAM_01.Model;
using Microsoft.EntityFrameworkCore;

namespace api_EXAM_01.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> products {  get; set; }
        public DbSet<Customer> customers {  get; set; }
        public DbSet<Category> categories {  get; set; }
        public DbSet<Order> orders {  get; set; }
    }
}
