using Hospital_OPD.Data;
using Hospital_OPD.DTO;
using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Hospital_OPD.Services.Implementation
{
    public class AppointmentService : IAppointmentServices, IReportService
    {
        private readonly AppDbContext _context;
        private readonly IMailService _emailService;

        public AppointmentService(AppDbContext context, IMailService emailService)
        {
            _context = context;

            //_logger = logger;
            //_emailService = emailService;
            _emailService = emailService;

        }
        public async Task<string> BookAppointment(Appointment appointment)
        {
            var doctor = await _context.Doctor.FindAsync(appointment.DoctorId);
            if (doctor == null) return "Doctor not found";

            if (doctor.IsOnLeave)
                return "Doctor is on leave";

            var time = appointment.AppointmentTime;
            var date = appointment.AppointmentDate;

            var morningStart = TimeOnly.Parse(doctor.MorningSlotStart);
            var morningEnd = TimeOnly.Parse(doctor.MorningSlotEnd);
            var eveningStart = TimeOnly.Parse(doctor.EveningSlotStart);
            var eveningEnd = TimeOnly.Parse(doctor.EveningSlotEnd);

            bool isInSlot =
                (time >= morningStart && time <= morningEnd) ||
                (time >= eveningStart && time <= eveningEnd);

            if (!isInSlot)
                return "Selected slot is not within doctor's availability.";

            bool slotTaken = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == doctor.Id &&
                a.AppointmentDate == date &&
                a.AppointmentTime >= time.AddMinutes(-29) &&
                a.AppointmentTime <= time.AddMinutes(29));

            if (slotTaken)
            {
                TimeOnly nextSlot = time.AddMinutes(30);
                while ((nextSlot < morningEnd && time < morningEnd) ||
                       (nextSlot < eveningEnd && time < eveningEnd))
                {
                    bool isNextSlotFree = !await _context.Appointments.AnyAsync(a =>
                        a.DoctorId == doctor.Id &&
                        a.AppointmentDate == date &&
                        a.AppointmentTime >= nextSlot.AddMinutes(-29) &&
                        a.AppointmentTime <= nextSlot.AddMinutes(29));

                    if (isNextSlotFree)
                        return $"Time already booked. Next available slot: {nextSlot:HH:mm}.";

                    nextSlot = nextSlot.AddMinutes(30);
                }

                return "No available slots left for today.";
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var patient = await _context.Patient.FindAsync(appointment.PatientId);

            if (patient != null && doctor != null)
            {
                await _emailService.SendAppointmentConfirmationEmailAsync(
                    patient.Email,
                    patient.Name,
                    doctor.Name,
                   DateOnly.FromDateTime(appointment.AppointmentDate), //
                    appointment.AppointmentTime
                );
            }

            return "Appointment booked successfully"; // ✅ This line was wrongly placed outside the method
        }


        public async Task<string> CancelAppointment(int patientId, int doctorId)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a =>
                a.PatientId == patientId &&
                a.DoctorId == doctorId);

            if (appointment == null)
            {
                return "There is No Booked Yet Appointment";
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return "Appointment cancelled successfully.";
        }

        public async Task<List<DailyAppointmentReportDto>> GetDailyAppointmentReport(DateTime date)
        {
            var DateParam = new SqlParameter("@Date", date);
            var result = await _context.Set<DailyAppointmentReportDto>()
             .FromSqlRaw("EXEC sp_GetDailyAppointmentCount @Date", DateParam)
        .ToListAsync();

            return result;
        }


        public async Task<List<PatientVisitMonthlyDto>> GetPatientVisitMonthlyAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(a => a.AppointmentDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.AppointmentDate <= endDate.Value);

            // Step 1: Fetch grouped raw data WITHOUT the ToString call inside the query
            var groupedData = await query
                .GroupBy(a => new
                {
                    a.PatientId,
                    a.Patient.Name,
                    Year = a.AppointmentDate.Year,
                    Month = a.AppointmentDate.Month
                })
                .Select(g => new
                {
                    PatientId = g.Key.PatientId,
                    PatientName = g.Key.Name,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    VisitCount = g.Count(),
                    VisitDatesRaw = g.Select(x => x.AppointmentDate).ToList()
                })
                .ToListAsync();

            // Step 2: Project into final DTO with string formatting (client-side)
            var result = groupedData
                .Select(g => new PatientVisitMonthlyDto
                {
                    PatientId = g.PatientId,
                    PatientName = g.PatientName,
                    VisitMonth = $"{g.Year}-{g.Month:D2}",
                    VisitCount = g.VisitCount,
                    VisitDates = string.Join(", ",
                        g.VisitDatesRaw
                         .OrderBy(d => d)
                         .Select(d => d.ToString("yyyy-MM-dd")))
                })
                .OrderBy(x => x.VisitMonth)
                .ThenBy(x => x.PatientName)
                .ToList();

            return result;
        }



    }

}

