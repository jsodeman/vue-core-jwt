using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace VueCoreJwt
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{environment}.json", optional: true)
				.Build();

			if (environment == "Development")
			{
				Log.Logger = new LoggerConfiguration()
					.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Debug)
					.Enrich.FromLogContext()
					.WriteTo.Console()
					.CreateLogger();
			}
			else
			{
				Log.Logger = new LoggerConfiguration()
					.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
					.Enrich.FromLogContext()
					.ReadFrom.Configuration(configuration)
					.CreateLogger();
			}

			try
			{
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application start-up failed");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config.Sources.Clear();

					var env = hostingContext.HostingEnvironment;

					config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, true)
						.AddEnvironmentVariables();

				})
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
		;
	}
}
