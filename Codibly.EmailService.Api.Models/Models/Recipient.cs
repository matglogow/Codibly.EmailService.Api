using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Codibly.EmailService.Api.Models.Models
{
    [ExcludeFromCodeCoverage]
    public class Recipient
    {
        #region Properties

        public virtual Email Email { get; set; }

        [MaxLength(100)]
        public string EmailAddress { get; set; }

        public int EmailId { get; set; }

        [Key]
        public int Id { get; set; }

        #endregion
    }
}
