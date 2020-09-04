using System.Collections.Generic;
using System.Threading.Tasks;
using Codibly.EmailService.Api.Dtos.Enums;
using Codibly.EmailService.Api.Dtos.Models;
using Codibly.EmailService.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Codibly.EmailService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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

        [HttpGet("{id}")]
        public async Task<EmailDto> Get(int id)
        {
            return await _emailService.GetEmail(id);
        }

        [HttpGet]
        public async Task<IEnumerable<EmailHeaderDto>> GetAll()
        {
            return await _emailService.GetAllEmails();
        }

        [HttpGet("{id}/state")]
        public async Task<EmailStateEnumDto> GetState(int id)
        {
            return await _emailService.GetEmailState(id);
        }

        [HttpPost]
        public async Task<EmailDto> Post([FromBody] EmailCreateableDto data)
        {
            return await _emailService.PostEmail(data);
        }

        [HttpPut("{id}")]
        public async Task<EmailDto> Put(int id, [FromBody] EmailDto data)
        {
            return await _emailService.UpdateEmail(id, data);
        }

        #endregion
    }
}
