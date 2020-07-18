using VueCoreJwt.App;

namespace VueCoreJwt.Models
{
	// Items from AppUser that aren't secret and useful client-side
	// This data is sent to Vue upon login or refresh
	// TODO: customize for your needs
	public class AuthResponse
	{
		public AuthResponse(AppConfig configuration, AppUser user, bool firstLogin)
		{
			Token = Jwt.GenerateTokenForUser(configuration, user);
			Id = user.Id;
			Name = user.Name;
			Email = user.Email;
			Role = user.Role;
			CustomInfo = user.CustomInfo;
			FirstLogin = firstLogin;
		}

		public int Id { get; set; }
		public string Token { get; set; }
		public string Role { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string CustomInfo { get; set; }
		public bool FirstLogin { get; set; }
	}
}
