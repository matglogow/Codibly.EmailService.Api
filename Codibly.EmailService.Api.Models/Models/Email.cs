using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Codibly.EmailService.Api.Models.Models.Enums;

namespace Codibly.EmailService.Api.Models.Models
{
    [ExcludeFromCodeCoverage]
    public class Email : EntityBase
    {
        #region Properties

        public string Content { get; set; }

        public virtual ICollection<Recipient> Recipients { get; set; }

        [MaxLength(100)]
        public string Sender { get; set; }

        public DateTimeOffset? SendOn { get; set; }

        public EmailStateEnum State { get; set; }

        [MaxLength(1000)]
        public string Subject { get; set; }

        #endregion
    }
}
