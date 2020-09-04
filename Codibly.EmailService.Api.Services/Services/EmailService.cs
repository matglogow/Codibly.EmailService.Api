using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Codibly.EmailService.Api.Dtos.Enums;
using Codibly.EmailService.Api.Dtos.Models;
using Codibly.EmailService.Api.Models;
using Codibly.EmailService.Api.Models.Models;
using Codibly.EmailService.Api.Services.Exceptions;
using Codibly.EmailService.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using EmailModel = Codibly.EmailService.Api.Models.Models.Email;

namespace Codibly.EmailService.Api.Services.Services
{
    public class EmailService : IEmailService
    {
        #region Member Variables

        private readonly EmailServiceDbContext _dbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Construction

        public EmailService(EmailServiceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        #region IEmailService Members

        public async Task<IEnumerable<EmailHeaderDto>> GetAllEmails()
        {
            var emails = await _dbContext.Emails.AsNoTracking().ToListAsync();

            return _mapper.Map<IEnumerable<EmailHeaderDto>>(emails);
        }

        public async Task<EmailStateEnumDto> GetEmailState(int id)
        {
            var email = await _dbContext.Emails
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            _ = email ?? throw new NotFoundException($"Email with id: {id} was not found");

            return _mapper.Map<EmailStateEnumDto>(email.State);
        }

        public async Task<EmailDto> GetEmail(int id)
        {
            var email = await _dbContext.Emails.AsNoTracking()
                .Include(e => e.Recipients)
                .FirstOrDefaultAsync(e => e.Id == id);

            _ = email ?? throw new NotFoundException($"Email with id: {id} was not found");

            return _mapper.Map<EmailDto>(email);
        }

        public async Task<EmailDto> PostEmail(EmailCreateableDto data)
        {
            _ = data ?? throw new ValidationException("Email data is empty");

            ValidateEmailRecipients(data.Recipients);

            var emailToCreate = _mapper.Map<EmailModel>(data);
            emailToCreate.Recipients = CreateEmailRecipients(emailToCreate, data.Recipients);

            var createdEmail = await _dbContext.Emails.AddAsync(emailToCreate);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EmailDto>(createdEmail.Entity);
        }

        public async Task<EmailDto> UpdateEmail(int id, EmailDto data)
        {
            _ = data ?? throw new ValidationException("Email data is empty");

            if (data.Id != id)
            {
                throw new ValidationException("Invalid email id value");
            }

            ValidateEmailRecipients(data.Recipients);

            var email = await _dbContext.Emails
                .Include(e => e.Recipients)
                .FirstOrDefaultAsync(e => e.Id == id);

            _ = email ?? throw new NotFoundException($"Email with id: {id} was not found");

            email.Content = data.Content;
            email.Subject = data.Subject;
            email.Recipients = CreateEmailRecipients(email, data.Recipients);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EmailDto>(email);
        }

        #endregion

        #region Private methods

        private ICollection<Recipient> CreateEmailRecipients(EmailModel email, ICollection<string> recipientList)
        {
            var recipients = new List<Recipient>(recipientList.Count);

            recipients.AddRange(recipientList.Select(recipientEmail => new Recipient
            {
                Email = email,
                EmailAddress = recipientEmail,
            }));

            return recipients;
        }

        private void ValidateEmailRecipients(ICollection<string> recipientList)
        {
            if (recipientList == null || !recipientList.Any())
            {
                throw new ValidationException("Invalid number of recipients. Min allowed recipients is: 1");
            }
        }

        #endregion
    }
}
