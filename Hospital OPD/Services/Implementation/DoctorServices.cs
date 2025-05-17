using Hospital_OPD.Data;
using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD.Services.Implementation
{
    public class DoctorServices : IDoctorServices
    {
        private readonly AppDbContext _context;
        
        public DoctorServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor> AddDoctor(Doctor doctor)
        {
            if (!TimeOnly.TryParse(doctor.MorningSlotStart, out _) ||
               !TimeOnly.TryParse(doctor.MorningSlotEnd, out _) ||
               !TimeOnly.TryParse(doctor.EveningSlotStart, out _) ||
               !TimeOnly.TryParse(doctor.EveningSlotEnd, out _))
            {
                throw new ArgumentException("Invalid time format in one of the slots.");
            }
            _context.Doctor.Add(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<List<Doctor>> GetAllDoctor()
        {
            var doctors = await _context.Doctor.Include(d => d.Department).ToListAsync();
            if (doctors == null)
            {
                return null;
            }
            return doctors;
        }

        public async Task<Doctor> GetDoctor(int id)
        {   
            var doctor = await _context.Doctor.Include(d=>d.Department).FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                return null;
            }
            return doctor;
        }

        public async Task<bool> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return false;
            }
            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDoctor(int id, Doctor doctor)
        {
            var existing = await _context.Doctor.FindAsync(id);
            if (existing == null) return false;
            existing.Name= doctor.Name;
            existing.Specialization = doctor.Specialization;
            existing.DepartmentId = doctor.DepartmentId;
            existing.IsOnLeave = doctor.IsOnLeave;
            existing.MorningSlotStart = doctor.MorningSlotStart;
            existing.MorningSlotEnd = doctor.MorningSlotEnd;
            existing.EveningSlotStart = doctor.EveningSlotStart;
            existing.EveningSlotEnd = doctor.EveningSlotEnd;
            await _context.SaveChangesAsync();
            return true;
        }

       
    }
}
