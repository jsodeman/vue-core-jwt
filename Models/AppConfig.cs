namespace VueCoreJwt.Models
{
	public class AppConfig
	{
		public string SiteUrl { get; set; }
		public string JwtKey { get; set; }
		public int JwtDays { get; set; }
		public string SomeServiceApiKey { get; set; }
		public string EmailServiceApiKey { get; set; }
		public bool ValidateEmail { get; set; }
		public bool IsDevelopment { get; set; }
	}
}
