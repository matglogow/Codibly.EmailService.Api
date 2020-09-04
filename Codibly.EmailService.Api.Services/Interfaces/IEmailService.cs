using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codibly.EmailService.Api.Dtos.Enums;
using Codibly.EmailService.Api.Dtos.Models;
using EmailModel = Codibly.EmailService.Api.Models.Models.Email;

namespace Codibly.EmailService.Api.Services.Interfaces
{
    public interface IEmailService
    {
        #region Inheritance

        Task<IEnumerable<EmailHeaderDto>> GetAllEmails();

        Task<ICollection<EmailModel>> GetAllPendingEmails();

        Task<EmailDto> GetEmail(int id);

        Task<EmailStateEnumDto> GetEmailState(int id);

        Task<EmailDto> PostEmail(EmailCreateableDto data);

        Task<EmailDto> UpdateEmail(int id, EmailDto data);

        Task UpdateEmailState(int id, DateTimeOffset sendOn);

        #endregion
    }
}
