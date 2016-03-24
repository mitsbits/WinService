using Atlas;
using Autofac;
using Common.Logging;
using System;
using System.Linq;

namespace WinService
{
    /// <summary>
    /// This represents the Windows Service application entity.
    /// </summary>
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// This represents the main entry point of the Windows Service application.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        private static void Main(string[] args)
        {
            try
            {
                var configuration = Host.UseAppConfig<Service>()
                                        .AllowMultipleInstances()
                                        .WithRegistrations(p => p.RegisterModule(new ServiceModule()));
                if (args != null && args.Any())
                    configuration = configuration.WithArguments(args);

                Host.Start(configuration);
            }
            catch (Exception ex)
            {
                Log.Fatal("Exception during startup.", ex);
                Console.ReadLine();
            }
        }
    }
}