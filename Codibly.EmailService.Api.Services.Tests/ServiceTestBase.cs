using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Codibly.EmailService.Api.Dtos;
using Codibly.EmailService.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Codibly.EmailService.Api.Services.Tests
{
    public abstract class ServiceTestBase<T, U> where T : DbSet<U> where U : class
    {
        #region Construction

        protected ServiceTestBase()
        {
            DbContext = CreateDefaultDbContext();
        }

        #endregion

        #region Properties

        public EmailServiceDbContext DbContext { get; }

        #endregion

        #region Inheritance

        protected virtual IMapper CreateDefaultMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            return mockMapper.CreateMapper();
        }

        protected virtual void FillDatabase(EmailServiceDbContext context, T table, ICollection<U> entities)
        {
            if (table.Any())
            {
                table.RemoveRange(table.ToList());
            }

            table.AddRange(entities);
            context.SaveChanges();
        }

        #endregion

        #region Private methods

        private EmailServiceDbContext CreateDefaultDbContext()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .AddEntityFrameworkProxies()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<EmailServiceDbContext>()
                .UseInMemoryDatabase("EmailServiceD")
                .UseLazyLoadingProxies()
                .UseInternalServiceProvider(serviceProvider)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new EmailServiceDbContext(options);
        }

        #endregion

        // protected virtual void FillAtlasDatabase<TEntity>(AtlasDbContext context, DbSet<TEntity> table, ICollection<TEntity> entities) where TEntity : class, IHashedEntity
        // {
        //     if (table.Any())
        //     {
        //         table.RemoveRange(table.ToList());
        //     }
        //
        //     table.AddRange(entities);
        //     context.SaveChanges();
        //
        //     var defaultHashService = CreateDefaultHashService();
        //     foreach (var entity in entities)
        //     {
        //         entity.HashValue = defaultHashService.HashEntity(entity);
        //     }
        //
        //     context.SaveChanges();
        // }
    }
}
