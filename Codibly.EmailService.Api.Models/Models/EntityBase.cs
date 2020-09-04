using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Codibly.EmailService.Api.Models.Models
{
    [ExcludeFromCodeCoverage]
    public abstract class EntityBase
    {
        #region Properties

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        [Key]
        public int Id { get; set; }

        #endregion
    }
}
