using System;
using System.Threading.Tasks;

public interface IMailService
{
    Task SendAppointmentConfirmationEmailAsync(string toEmail, string patientName, string doctorName, DateOnly date, TimeOnly time);
}
