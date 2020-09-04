using System.Collections.Generic;
using System.Threading.Tasks;
using Codibly.EmailService.Api.Dtos.Enums;
using Codibly.EmailService.Api.Dtos.Models;

namespace Codibly.EmailService.Api.Services.Interfaces
{
    public interface IEmailService
    {
        #region Inheritance

        Task<IEnumerable<EmailHeaderDto>> GetAllEmails();

        Task<EmailDto> GetEmail(int id);

        Task<EmailStateEnumDto> GetEmailState(int id);

        Task<EmailDto> PostEmail(EmailCreateableDto data);

        Task<EmailDto> UpdateEmail(int id, EmailDto data);

        #endregion
    }
}
