using System.Collections.Generic;

namespace Codibly.EmailService.Api.Dtos.Dtos
{
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
