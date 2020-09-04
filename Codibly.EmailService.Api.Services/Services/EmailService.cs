using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            // TODO: Implement proper exception
            _ = data ?? throw new Exception();

            if (data.Recipients == null || !data.Recipients.Any())
            {
                // TODO: Implement proper exception
                throw new Exception();
            }

            var emailToCreate = _mapper.Map<EmailModel>(data);
            var recipients = new List<Recipient>(data.Recipients.Count);
            recipients.AddRange(data.Recipients.Select(recipientEmail => new Recipient
            {
                Email = emailToCreate,
                EmailAddress = recipientEmail,
            }));

            emailToCreate.Recipients = recipients;

            var createdEmail = await _dbContext.Emails.AddAsync(emailToCreate);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EmailDto>(createdEmail.Entity);
        }

        #endregion
    }
}
