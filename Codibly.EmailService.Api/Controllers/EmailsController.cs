using System.Collections.Generic;
using System.Threading.Tasks;
using Codibly.EmailService.Api.Dtos.Dtos;
using Codibly.EmailService.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Codibly.EmailService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class EmailsController : ControllerBase
    {
        #region Member Variables

        private readonly IEmailService _emailService;

        #endregion

        #region Construction

        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        #endregion

        #region Public methods

        [HttpGet]
        public async Task<IEnumerable<EmailHeaderDto>> GetAll()
        {
            return await _emailService.GetAllEmails();
        }

        [HttpPost]
        public async Task<EmailDto> Post([FromBody] EmailCreateableDto data)
        {
            return await _emailService.PostEmail(data);
        }

        #endregion
    }
}
