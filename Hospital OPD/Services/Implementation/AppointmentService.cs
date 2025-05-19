using Hospital_OPD.Data;
using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD.Services.Implementation
{
    public class AppointmentService : IAppointmentServices
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
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
            if (!isInSlot) return "Selected Slot is not within range.";

            bool SlotTaken = await _context.Appointments.AnyAsync(a =>
                    a.DoctorId == doctor.Id &&
                    a.AppointmentDate == date &&
                    a.AppointmentTime >= time.AddMinutes(-29) && // Prevents bookings 1 min before
                    a.AppointmentTime <= time.AddMinutes(29));   //
            if (SlotTaken)
            {
                TimeOnly nextSlot = time.AddMinutes(30);
                while((nextSlot < morningEnd && time < morningEnd)||
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


            return "Appointment booked successfully";
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

    }
}
