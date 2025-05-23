using Hospital_OPD.DTO;
using Hospital_OPD.Model;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<DailyAppointmentReportDto> DailyAppointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyAppointmentReportDto>().HasNoKey();
        }
    }
}
