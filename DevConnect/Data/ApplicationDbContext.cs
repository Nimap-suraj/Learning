using DevConnect.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization.DataContracts;

namespace DevConnect.Data
{
    public class ApplicationDbContext : DbContext
    {
        public  ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
