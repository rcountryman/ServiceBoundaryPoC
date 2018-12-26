using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ToDo.Worker
{
	internal class Program
	{
		private static Task Main(string[] args) =>
			// TODO: Port to .NET Core 3.0 and use Host.CreateDefaultBuilder(args)
			new HostBuilder()
				.ConfigureHostConfiguration(configHost =>
				{
					configHost.SetBasePath(Directory.GetCurrentDirectory());
					configHost.AddJsonFile("hostsettings.json", optional: true);
					configHost.AddEnvironmentVariables(prefix: "PREFIX_");
					configHost.AddCommandLine(args);
				})
				.ConfigureAppConfiguration((hostContext, configApp) =>
				{
					configApp.AddJsonFile("appsettings.json", optional: true);
					configApp.AddJsonFile(
						$"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
						optional: true);
					configApp.AddEnvironmentVariables(prefix: "PREFIX_");
					configApp.AddCommandLine(args);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<LifetimeEventsHostedService>();
				})
				.ConfigureLogging((hostContext, configLogging) =>
				{
					configLogging.AddConsole();
					configLogging.AddDebug();
				})
				.UseConsoleLifetime()
				.Build()
				.RunAsync();
	}
}
