using System.Collections.Generic;
using System.Threading.Tasks;
using Codibly.EmailService.Api.Dtos.Dtos;

namespace Codibly.EmailService.Api.Services.Interfaces
{
    public interface IEmailService
    {
        #region Inheritance

        Task<IEnumerable<EmailHeaderDto>> GetAllEmails();

        Task<EmailDto> PostEmail(EmailCreateableDto data);

        #endregion
    }
}
