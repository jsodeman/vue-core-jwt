using System.Threading.Tasks;
using Serilog;
using VueCoreJwt.Interfaces;
using VueCoreJwt.Models;

// TODO: customize for your needs and integrate with PostmarkApp, Sendgrid, etc
namespace VueCoreJwt.App
{
	public class EmailService : IEmailService
	{
		private readonly AppConfig config;

		public EmailService(AppConfig config)
		{
			this.config = config;
		}

		public Task Register(AppUser user)
		{
			var activationLink = $"{config.SiteUrl}/confirm/{user.EmailToken}";

			if (config.IsDevelopment)
			{
				Log.Information($"Registration Confirmation Link: {activationLink}");
			}
			// TODO: send an email to the user with the link
			// config.EmailServiceApiKey
			return Task.CompletedTask;
		}

		public Task Reset(AppUser user)
		{
			var resetLink = $"{config.SiteUrl}/reset-confirm/{user.EmailToken}";

			if (config.IsDevelopment)
			{
				Log.Information($"Password Reset Link: {resetLink}");
			}
			// TODO: send an email to the user with the link
			// config.EmailServiceApiKey

			return Task.CompletedTask;
		}
	}
}
