using Hospital_OPD.Data;
using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD.Services.Implementation
{
    public class MedicalService : IMedicalRecord
    {
        private readonly AppDbContext _context;
        public MedicalService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<string> AddMedicalHistory(MedicalRecord history)
        {
            // Check if the patient had an appointment with the doctor on that date
            bool hasAppointment = await _context.Appointments.AnyAsync(a =>
                a.PatientId == history.PatientID &&
                a.DoctorId == history.DoctorId &&
                a.AppointmentDate.Date == history.VisitDate.Date
            );

            if (!hasAppointment)
            {
                return "Error: No appointment found for this patient with the doctor on the given date.";
            }

            _context.MedicalRecords.Add(history);
            await _context.SaveChangesAsync();
            return "Medical history added successfully";
        }
        public async Task<List<MedicalRecord>> GetMedicalHistory(int patientId)
        {
            var history = await _context.MedicalRecords
                .Include(h => h.Doctor)
                .Where(h => h.PatientID == patientId)
                .OrderByDescending(h => h.VisitDate)
                .ToListAsync();

            return history ?? new List<MedicalRecord>();
        }


    }
}
