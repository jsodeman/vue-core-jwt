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

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var config = new AppConfig();
			config.IsDevelopment = Env.IsDevelopment();
			Configuration.Bind("App", config);
			services.AddSingleton(config);

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


			// services.AddAuthentication(options =>
			// {
			// 	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			// 	options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
			// 	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			// }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			// {
			// 	options.RequireHttpsMetadata = true;
			// 	options.SaveToken = true;
			// 	options.TokenValidationParameters = new TokenValidationParameters
			// 	{
			// 		//  .  }; });

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

			services.AddScoped<IDatabase, Database>(ctx => new Database());
			services.AddTransient<IEmailService, EmailService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseCors("_vueDev");
				app.UseDeveloperExceptionPage();
			}else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseCookiePolicy(new CookiePolicyOptions
			{
				MinimumSameSitePolicy = env.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
				HttpOnly = HttpOnlyPolicy.Always,
				Secure = CookieSecurePolicy.Always
			});

			app.UseSerilogRequestLogging();

			app.UseRouting();

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
