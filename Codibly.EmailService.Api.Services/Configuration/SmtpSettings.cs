using System.Security;

namespace Codibly.EmailService.Api.Services.Configuration
{
    public class SmtpSettings
    {
        #region Properties

        // Should be store e.g. with Secret Manager tool
        public string Password { get; set; }

        public int Port { get; set; }

        public string Server { get; set; }

        public string Username { get; set; }

        #endregion
    }
}
