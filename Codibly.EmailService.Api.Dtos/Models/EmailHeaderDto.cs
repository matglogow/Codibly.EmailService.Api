using System;
using System.Diagnostics.CodeAnalysis;
using Codibly.EmailService.Api.Dtos.Enums;

namespace Codibly.EmailService.Api.Dtos.Models
{
    [ExcludeFromCodeCoverage]
    public class EmailHeaderDto
    {
        #region Properties

        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int Id { get; set; }

        public EmailStateEnumDto State { get; set; }

        public string Subject { get; set; }

        #endregion
    }
}
