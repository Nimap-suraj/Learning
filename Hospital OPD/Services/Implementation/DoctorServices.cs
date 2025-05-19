using System.Drawing;
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
            try
            {
                // Normalize and validate time strings
                doctor.MorningSlotStart = TimeOnly.Parse(doctor.MorningSlotStart).ToString("HH:mm");
                doctor.MorningSlotEnd = TimeOnly.Parse(doctor.MorningSlotEnd).ToString("HH:mm");
                doctor.EveningSlotStart = TimeOnly.Parse(doctor.EveningSlotStart).ToString("HH:mm");
                doctor.EveningSlotEnd = TimeOnly.Parse(doctor.EveningSlotEnd).ToString("HH:mm");
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid time format. Please use HH:mm (e.g., 10:30).");
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

        public async Task<Doctor> UpdateDoctor(int id, Doctor doctor)
        {
            var existingDoctor = await _context.Doctor.FindAsync(id);
            if (existingDoctor == null)
                throw new KeyNotFoundException("Doctor not found");

            existingDoctor.Name = doctor.Name;
            existingDoctor.DepartmentId = doctor.DepartmentId;
            existingDoctor.Specialization = doctor.Specialization;
            existingDoctor.IsOnLeave = doctor.IsOnLeave;

            try
            {
                existingDoctor.MorningSlotStart = TimeOnly.Parse(doctor.MorningSlotStart).ToString("HH:mm");
                existingDoctor.MorningSlotEnd = TimeOnly.Parse(doctor.MorningSlotEnd).ToString("HH:mm");
                existingDoctor.EveningSlotStart = TimeOnly.Parse(doctor.EveningSlotStart).ToString("HH:mm");
                existingDoctor.EveningSlotEnd = TimeOnly.Parse(doctor.EveningSlotEnd).ToString("HH:mm");
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid time format. Please use HH:mm.");
            }

            await _context.SaveChangesAsync();
            return existingDoctor;
        }

        public async Task<string> MarkDoctorLeave(int doctorId, bool isOnLeave)
        {
            var doctor = await _context.Doctor.FindAsync(doctorId);
            if (doctor == null)
            {
                return "Doctor not found.";
            }
            doctor.IsOnLeave = isOnLeave;
            _context.Doctor.Update(doctor);
            await _context.SaveChangesAsync();

            return $"Doctor leave status updated to {(isOnLeave ? "On Leave" : "Available")}.";
        }

        public async Task<Dictionary<string, string>> GetDoctorScheduleWithNames(int doctorId, DateTime date)
        {
            var doctor = await _context.Doctor.FindAsync(doctorId);
            if (doctor == null) return new Dictionary<string, string> { { "Error", "Doctor not found" } };

            if (doctor.IsOnLeave)
                return new Dictionary<string, string> { { "Notice", "Doctor is on leave" } };

            var morningStart = TimeOnly.Parse(doctor.MorningSlotStart);
            var morningEnd = TimeOnly.Parse(doctor.MorningSlotEnd);
            var EveningStart = TimeOnly.Parse(doctor.EveningSlotStart);
            var EveningEnd = TimeOnly.Parse(doctor.EveningSlotEnd);

            var allSlots = new List<TimeOnly>();
            for (var t = morningStart; t < morningEnd; t = t.AddMinutes(30))
            {
                allSlots.Add(t);
            }
            for (var t = EveningStart; t < EveningEnd; t = t.AddMinutes(30))
            {
                allSlots.Add(t);
            }
            var bookedAppointments = await _context.Appointments.Where(a =>

            a.DoctorId == doctorId  &&
            a.AppointmentDate.Date == date.Date)
                .Include(p=>p.Patient)
                .ToListAsync()
                ;
            var slotMap = new Dictionary<string, string>();

            foreach (var slot in allSlots)
            {
                var appointment = bookedAppointments.FirstOrDefault(a => a.AppointmentTime == slot);
                if (appointment != null)
                {
                    slotMap[slot.ToString("HH:mm")] = appointment.Patient?.Name ?? "Booked";
                }
                else
                {
                    slotMap[slot.ToString("HH:mm")] = "Available";
                }
            }

            return slotMap;
        }

    }






}
