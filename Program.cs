using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using VueCoreJwt.App;
using VueCoreJwt.Data;
using VueCoreJwt.Interfaces;
using VueCoreJwt.Models;

namespace VueCoreJwt
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var env = builder.Environment;

			if (env.IsDevelopment())
			{
				// allow the CORS access in development for use by the vue dev server
				builder.Services.AddCors(options =>
				{
					options.AddPolicy("_vueDev",
						builder =>
						{
							builder.WithOrigins("http://localhost:8080")
								.AllowCredentials()
								.AllowAnyHeader()
								.AllowAnyMethod();
						});
				});
			}

			// register the AppConfig class for DI
			var config = new AppConfig();
			builder.Configuration.Bind("App", config);
			config.IsDevelopment = env.IsDevelopment();
			builder.Services.AddSingleton(config);

			// use JWT authentication
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = true;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateAudience = true,
						ValidateIssuer = true,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.JwtKey)),
						ValidIssuer = config.SiteUrl,
						ValidAudience = config.SiteUrl
					};
				});

			builder.Services.AddAuthorization();

			builder.Services.AddControllers();

			// TODO: replace with your database and email service
			builder.Services.AddScoped<IDatabase, Database>(ctx => new Database());
			builder.Services.AddTransient<IEmailService, EmailService>();

			// configure Serilog
			// TODO: adjust the logging levels and desired (also in appsettings.config)
			// TODO: change the sink to use a database or other option
			builder.Host.UseSerilog((hostingContext, config) =>
			{
				if (env.IsDevelopment())
				{
					config
						.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Debug)
						.Enrich.FromLogContext()
						.WriteTo.Console()
						;
				}
				else
				{
					config
						.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
						.Enrich.FromLogContext()
						.ReadFrom.Configuration(builder.Configuration)
						;
				}
			});

			var app = builder.Build();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			// this serves the production files from /dist
			// TODO: see web.config for information or to change redirects
			app.UseStaticFiles();

			if (env.IsDevelopment())
			{
				app.UseCors("_vueDev");
			}

			// security restrictions on the cookie
			app.UseCookiePolicy(new CookiePolicyOptions
			{
				MinimumSameSitePolicy = env.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
				HttpOnly = HttpOnlyPolicy.Always,
				Secure = CookieSecurePolicy.Always
			});

			// middleware to insert the JWT from the cookie into the request header
			app.UseSecureJwt();

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();

			// log API requests
			app.UseSerilogRequestLogging();

			try
			{
				app.Run();
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
	}
}
