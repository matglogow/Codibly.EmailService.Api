using Codibly.EmailService.Api.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Codibly.EmailService.Api.Models
{
    public class EmailServiceDbContext : DbContext
    {
        #region Construction

        public EmailServiceDbContext(DbContextOptions<EmailServiceDbContext> options) : base(options)
        {
        }

        #endregion

        #region Properties

        public DbSet<Email> Emails { get; set; }

        public DbSet<Recipient> Recipients { get; set; }

        #endregion

        #region Inheritance

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Email>()
                .HasMany(e => e.Recipients)
                .WithOne(r => r.Email)
                .HasForeignKey(r => r.EmailId);
        }

        #endregion
    }
}
