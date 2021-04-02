using common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using service;
using System;
using System.IO;

namespace app
{
    public static class Program
    {

        // https://medium.com/@lschoeneman/how-to-take-advantage-of-entityframework-core-in-net-core-2-2-console-applications-d76417ded5eb

        #region Constants

        private const string CONNECTION_STRING_NAME = "CreditCardDatabase";
        private const string LOG_DIR = "logs";

        #endregion

        #region Properties

        private static IApplicationLogger Logger { get { return GetApplicationLogger(); } }

        #endregion

        #region Main Entry Point

        public static void Main(string[] args)
        {
            try
            {
                InitializeApplicationEnvironment(args);

                IServiceCollection services = ConfigureServices();
                ServiceProvider serviceProvider = services.BuildServiceProvider();
                serviceProvider.GetService<ConsoleApplication>().Run();
            }
            catch (Exception ex)
            {
                Logger.LogError("An un expected error has occurred...");
                Logger.LogError(ex);
            }

#if DEBUG
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
#endif
        }

        #endregion

        #region Helper Methods

        public static IConfiguration LoadConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        private static IServiceCollection ConfigureServices()
        {
            Logger.LogInfo("Initializing services...");

            IServiceCollection services = new ServiceCollection();

            IConfiguration config = LoadConfiguration();
            services.AddSingleton(config);
            services.AddTransient<ICreditCardService, CreditCardService>();
            services.AddDbContext<CreditCardContext>(o => o.UseMySQL(config.GetConnectionString(CONNECTION_STRING_NAME)));
            services.AddTransient<ConsoleApplication>();

            return services;
        }

        private static string GetLogEnvironmentVariables(string key, string defaultValue)
        {
            try
            {
                string value = Environment.GetEnvironmentVariable(key);
                if (value == null)
                {
                    Logger.LogWarn($"ENVVAR with key [{key}] not found. Using default value of [{defaultValue}].");
                    return defaultValue;
                }
                else
                {
                    Logger.LogInfo($"ENVVAR with key [{key}] found. Using configured value of [{value}].");
                    return value;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return defaultValue;
            }
        }

        private static void InitializeApplicationEnvironment(string[] args)
        {
            Logger.Initialize(LOG_DIR, args);
            ApplicationEnvironment.Singleton.Initialize();
        }

        #endregion

        #region Helper Methods

        private static IApplicationLogger GetApplicationLogger()
        {
            IApplicationLogger logger = ApplicationLogger.Singleton;
            logger.SetType(typeof(Program));
            return logger;
        }

        #endregion

    }
}
