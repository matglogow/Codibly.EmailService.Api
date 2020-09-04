using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Codibly.EmailService.Api.Dtos.Enums;

namespace Codibly.EmailService.Api.Dtos.Models
{
    [ExcludeFromCodeCoverage]
    public class EmailDto
    {
        #region Properties

        public string Content { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int Id { get; set; }

        public virtual ICollection<string> Recipients { get; set; }

        public string Sender { get; set; }
        public DateTimeOffset SendOn { get; set; }

        public EmailStateEnumDto State { get; set; }

        public string Subject { get; set; }

        #endregion
    }
}
