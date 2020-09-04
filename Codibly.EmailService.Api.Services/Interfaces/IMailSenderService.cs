using System.Threading.Tasks;

namespace Codibly.EmailService.Api.Services.Interfaces
{
    public interface IMailSenderService
    {
        Task SendAllPendingEmails();
    }
}
