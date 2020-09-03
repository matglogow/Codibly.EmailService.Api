using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Codibly.EmailService.Api
{
    public class Program
    {
        #region Public methods

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        #endregion
    }
}
