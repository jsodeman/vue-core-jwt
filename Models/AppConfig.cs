namespace VueCoreJwt.Models
{
	// this is hydrated from appsettings.json
	// TODO: customize for your needs
	// TODO: set ValidateEmail in appsettings to control the registration flow
	// TODO: update the dev and production settings in appsettings.json
	public class AppConfig
	{
		public string SiteUrl { get; set; }
		public string JwtKey { get; set; }
		public int JwtDays { get; set; }
		public string SomeServiceApiKey { get; set; }
		public string EmailServiceApiKey { get; set; }
		public bool ValidateEmail { get; set; }	// if true then new users must confirm their email before gaining access
		public bool IsDevelopment { get; set; }
		public string CookieName { get; set; }	// this can be any value
	}
}
