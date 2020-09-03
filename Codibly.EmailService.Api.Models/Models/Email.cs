using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codibly.EmailService.Api.Models.Models.Enums;

namespace Codibly.EmailService.Api.Models.Models
{
    public class Email : EntityBase
    {
        #region Properties

        public string Content { get; set; }

        public virtual ICollection<Recipient> Recipients { get; set; }

        [MaxLength(100)]
        public string Sender { get; set; }

        public EmailStateEnum State { get; set; }

        [MaxLength(1000)]
        public string Subject { get; set; }

        #endregion
    }
}
