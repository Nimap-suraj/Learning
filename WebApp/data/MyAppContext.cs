using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options):base(options) { }   
        
        public DbSet<Item> Items { get; set; } 
    }
}
