using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service;
using System.IO;

namespace Interface
{
    public static class Program
    {

        // https://medium.com/@lschoeneman/how-to-take-advantage-of-entityframework-core-in-net-core-2-2-console-applications-d76417ded5eb

        #region Constants

        private const string CONNECTION_STRING_NAME = "CreditCardDatabase";

        #endregion

        #region Main Entry Point

        public static void Main(string[] args)
        {
            IServiceCollection services = ConfigureServices();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ConsoleApplication>().Run();
        }

        #endregion

        #region Helper Methods

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            IConfiguration config = LoadConfiguration();
            services.AddSingleton(config);
            services.AddTransient<ICreditCardService, CreditCardService>();
            services.AddDbContext<CreditCardContext>(o => o.UseMySQL(config.GetConnectionString(CONNECTION_STRING_NAME)));
            services.AddTransient<ConsoleApplication>();

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConfiguration(config.GetSection("Logging"))
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Interface.Program", LogLevel.Debug)
                    .AddConsole();
            });

            ApplicationLogger.LoggerFactory = loggerFactory;
            loggerFactory.CreateLogger("Program").LogInformation("Initializing services...");

            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        #endregion

    }
}
