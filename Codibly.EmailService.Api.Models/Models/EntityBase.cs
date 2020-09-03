using System;
using System.ComponentModel.DataAnnotations;

namespace Codibly.EmailService.Api.Models.Models
{
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
