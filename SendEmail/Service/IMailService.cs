using System.Threading.Tasks;
using SendEmail.Model;
namespace SendEmail.Service
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}