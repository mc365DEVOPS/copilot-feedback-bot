﻿using Microsoft.Extensions.Logging;
using System.Configuration;

namespace Common.DataUtils
{
    /// <summary>
    /// Provides utilities for console apps.
    /// </summary>
    public class ConsoleApp
    {

        public static void WebjobWait(ILogger telemetry)
        {
            telemetry.LogInformation("Waiting 10 mins...");
            System.Threading.Thread.Sleep(600000); // 10 mins
        }

        /// <summary>
        /// Exit app. 
        /// </summary>
        public static void BombOut(bool error)
        {
#if DEBUG
            Console.WriteLine("\nDEBUG MODE: All done. Press any key to continue.");
            Console.ReadKey();
#endif
            if (error)
            {
                Environment.Exit(-1);
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public static void PrintStartupAndLoggingConfig(ILogger debugTracer)
        {
            if (debugTracer is null)
            {
                throw new ArgumentNullException(nameof(debugTracer));
            }

            var buildLabel = ConfigurationManager.AppSettings["BuildLabel"];
            debugTracer.LogInformation($"Office 365 Advanced Analytics engine START: '{buildLabel}'.");

            string efConnectionString = ConfigurationManager.ConnectionStrings["SPOInsightsEntities"].ConnectionString;
            var sqlConnectionInfo = new System.Data.SqlClient.SqlConnectionStringBuilder(efConnectionString);
            debugTracer.LogInformation($"Destination SQL Server='{sqlConnectionInfo.DataSource}', DB='{sqlConnectionInfo.InitialCatalog}'.");

            bool loggingEnabled = ConfigurationManager.AppSettings["ImportLogging"] == "True";
#if DEBUG
            loggingEnabled = true;
#endif

            if (loggingEnabled)
            {
                debugTracer.LogInformation("Import logging is ENABLED.");
            }
            else
            {
                debugTracer.LogInformation("Import logging is disabled. Add key 'ImportLogging' value 'True' to configuration to enable full logging.");
            }
        }

    }
}
