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

        private const string ENV_KEY_LOG_DIR = "LOG_DIR";
        private const string ENV_VALUE_LOG_DIR = "logs";
        private const string CONNECTION_STRING_NAME = "CreditCardDatabase";

        #endregion

        #region Main Entry Point

        public static void Main(string[] args)
        {
            try
            {
                InitializeApplicationEnvironment();

                IServiceCollection services = ConfigureServices();
                ServiceProvider serviceProvider = services.BuildServiceProvider();
                serviceProvider.GetService<ConsoleApplication>().Run();

#if DEBUG
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
#endif
            }
            catch (Exception ex)
            {
                ApplicationLogger.Singleton.LogError("An un expected error has occurred...");
                ApplicationLogger.Singleton.LogError(ex);
            }
        }

        #endregion

        #region Helper Methods

        private static IServiceCollection ConfigureServices()
        {
            ApplicationLogger.Singleton.LogInfo("Initializing services...");

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
                    ApplicationLogger.Singleton.LogWarn("ENVVAR with key [{0}] not found. Using default value of [{1}].", key, defaultValue);
                    return defaultValue;
                }
                else
                {
                    ApplicationLogger.Singleton.LogInfo("ENVVAR with key [{0}] found. Using configured value of [{1}].", key, value);
                    return value;
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Singleton.LogError(ex);
                return defaultValue;
            }
        }

        private static void InitializeApplicationEnvironment()
        {
            string logDir = GetLogEnvironmentVariables(ENV_KEY_LOG_DIR, ENV_VALUE_LOG_DIR);
            ApplicationLogger.Singleton.Initialize(logDir);
            ApplicationEnvironment.Singleton.Initialize();
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
