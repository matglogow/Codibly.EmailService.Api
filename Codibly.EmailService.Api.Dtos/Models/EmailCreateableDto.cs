using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Codibly.EmailService.Api.Dtos.Models
{
    [ExcludeFromCodeCoverage]
    public class EmailCreateableDto
    {
        #region Properties

        public string Content { get; set; }

        public virtual ICollection<string> Recipients { get; set; }

        public string Sender { get; set; }

        public string Subject { get; set; }

        #endregion
    }
}
