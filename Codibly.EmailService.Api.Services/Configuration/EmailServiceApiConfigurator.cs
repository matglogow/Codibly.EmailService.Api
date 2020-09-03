using System.Configuration;
using AutoMapper;
using Codibly.EmailService.Api.Dtos;
using Codibly.EmailService.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Codibly.EmailService.Api.Services.Configuration
{
    public static class EmailServiceApiConfigurator
    {
        #region Public methods

        public static void ConfigureApi(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Codibly Email Service - Public API");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            MigrateDatabase(app);
        }

        public static void ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            ValidateConfiguration(configuration);

            services.AddControllers();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddDbContext<EmailServiceDbContext>(options =>
            {
                options.UseSqlite(configuration[Constants.CONNECTION_STRING_CONFIG_KEY],
                    sqlLiteOptions =>
                    {
                        sqlLiteOptions.MigrationsAssembly(configuration[Constants.MIGRATION_ASSEMBLY_CONFIG_KEY]);
                    });
                options.UseLazyLoadingProxies();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"Codibly Email Service - Public API",
                    Version = "v1"
                });
            });
        }

        #endregion

        #region Private methods

        private static void MigrateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var dbContext = serviceScope.ServiceProvider.GetService<EmailServiceDbContext>();

            dbContext?.Database.Migrate();
        }

        private static void ValidateConfiguration(IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration[Constants.CONNECTION_STRING_CONFIG_KEY]))
            {
                throw new ConfigurationErrorsException("Connection string not provided");
            }

            if (string.IsNullOrEmpty(configuration[Constants.MIGRATION_ASSEMBLY_CONFIG_KEY]))
            {
                throw new ConfigurationErrorsException("Database migration assembly not provided");
            }
        }

        #endregion
    }
}
