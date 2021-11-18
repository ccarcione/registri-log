using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Serilog;
using net_registri_log.Logs.Models;

namespace net_registri_log.Logs
{
    public class Logger
    {
        private static string optionsJsonKey = "net-registri-log:Logs.Options";

        public static void Initialize()
        {
            var configuration = new ConfigurationBuilder()
              .AddJsonFile($"appsettings.json", false, true)
              .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", false, true)
              .AddEnvironmentVariables()
              .Build();

            Options options = configuration.GetSection(optionsJsonKey).Get<Options>();
            if (options.SerilogDebuggingSelfLogEnable)
            {
                Serilog.Log.Debug($"Serilog.Debugging.SelfLog: {options.SerilogSelfFileName}");
                Serilog.Log.Debug("Remember to enable file/folder access permissions.");
                StreamWriter file;
                if (!File.Exists(options.SerilogSelfFileName))
                {
                    file = File.CreateText("SerilogSelf.log");
                }
                else
                {
                    file = File.AppendText(options.SerilogSelfFileName);
                }
                Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));
            }

            if (options.Enable)
            {
                Serilog.Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
            }
        }
    }
}
