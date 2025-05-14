using Microsoft.EntityFrameworkCore;

namespace CRUD_REVISION.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }  
        
        DbSet<User> Users { get; set; } 
      
    }
}
