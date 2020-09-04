using System.Threading.Tasks;
using Codibly.EmailService.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Codibly.EmailService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class MailSenderController : ControllerBase
    {
        #region Member Variables

        private readonly IMailSenderService _mailSenderService;

        #endregion

        #region Construction

        public MailSenderController(IMailSenderService mailSenderService)
        {
            _mailSenderService = mailSenderService;
        }

        #endregion

        #region Public methods

        [HttpPost]
        public async Task SendAllPendingEmails()
        {
            await _mailSenderService.SendAllPendingEmails();
        }

        #endregion
    }
}
