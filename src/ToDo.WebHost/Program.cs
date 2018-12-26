using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Host = Microsoft.AspNetCore.WebHost;

namespace ToDo.WebHost
{
	public class Program
	{
		// TODO: Port to .NET Core 3.0 and use the Generic Web Host Builder and remove the Startup assembly entirely
		public static Task Main(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build()
				.RunAsync();
	}
}
