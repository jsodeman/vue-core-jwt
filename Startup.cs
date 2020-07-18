using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using VueCoreJwt.App;
using VueCoreJwt.Data;
using VueCoreJwt.Interfaces;
using VueCoreJwt.Models;

namespace VueCoreJwt
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Env { get; }

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			Env = env;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// register and fill the config class
			var config = new AppConfig();
			Configuration.Bind("App", config);
			config.IsDevelopment = Env.IsDevelopment();
			services.AddSingleton(config);

			// use JWT authentication
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

			// while in dev allow calls to the API from the Vue dev server running on a different URL
			services.AddCors(options =>
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

			services.AddControllers();

			// TODO: replace with your database and email service
			services.AddScoped<IDatabase, Database>(ctx => new Database());
			services.AddTransient<IEmailService, EmailService>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				// only allow the CORS access in development
				app.UseCors("_vueDev");
				app.UseDeveloperExceptionPage();
			}else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			// this serves the production files from /dist
			// TODO: see web.config for information or to change redirects
			app.UseStaticFiles();

			// security restrictions on the cookie
			app.UseCookiePolicy(new CookiePolicyOptions
			{
				MinimumSameSitePolicy = env.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
				HttpOnly = HttpOnlyPolicy.Always,
				Secure = CookieSecurePolicy.Always
			});

			// log API requests
			app.UseSerilogRequestLogging();

			app.UseRouting();

			// middleware to insert the JWT from the cookie into the request header
			// https://geeks-world.github.io/articles/468401/index.html
			app.UseSecureJwt();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
