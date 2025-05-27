using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;

    public MailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendAppointmentConfirmationEmailAsync(string toEmail, string patientName, string doctorName, DateOnly date, TimeOnly time)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "Appointment Confirmation";

        var builder = new BodyBuilder
        {
            HtmlBody = $"<p>Dear {patientName},</p>" +
                       $"<p>Your appointment with Dr. {doctorName} has been successfully booked.</p>" +
                       $"<p><strong>Date:</strong> {date:dd-MM-yyyy}<br>" +
                       $"<strong>Time:</strong> {time:hh\\:mm}</p>" +
                       $"<p>Thank you for choosing our clinic.</p>"
        };

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}
