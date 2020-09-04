using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Codibly.EmailService.Api.Models.Models;
using Codibly.EmailService.Api.Services.Configuration;
using Codibly.EmailService.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Codibly.EmailService.Api.Services.Services
{
    public class MailSenderService : IMailSenderService
    {
        #region Member Variables

        private readonly IEmailService _emailService;
        private readonly SmtpSettings _smtpSettings;

        #endregion

        #region Construction

        public MailSenderService(IOptions<SmtpSettings> smtpSettings, IEmailService emailService)
        {
            _smtpSettings = smtpSettings.Value;
            _emailService = emailService;
        }

        #endregion

        #region IMailSenderService Members

        public async Task SendAllPendingEmails()
        {
            var emails = await _emailService.GetAllPendingEmails();

            if (emails.Count == 0)
            {
                return;
            }

            using SmtpClient client = ConfigureSmtpClient();

            foreach (Email email in emails)
            {
                MailMessage emailMessage = CreateEmailMessage(email);
                await client.SendMailAsync(emailMessage);

                await _emailService.UpdateEmailState(email.Id, DateTimeOffset.UtcNow);

                emailMessage.Dispose();
            }
        }

        #endregion

        #region Private methods

        private SmtpClient ConfigureSmtpClient()
        {
            return new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential
                {
                    UserName = _smtpSettings.Username,
                    Password = _smtpSettings.Password
                }
            };
        }

        private MailMessage CreateEmailMessage(Email email)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(email.Sender),
                Subject = email.Subject,
                Body = email.Content
            };

            email.Recipients.ForAll(emailRecipient => mailMessage.To.Add(new MailAddress(emailRecipient.EmailAddress)));

            return mailMessage;
        }

        #endregion
    }
}
