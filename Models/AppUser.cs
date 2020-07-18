using System;
using System.Linq;
using System.Security.Claims;

namespace VueCoreJwt.Models
{
	// TODO: customize for your needs
	public class AppUser
	{
		public int Id { get; set; }

        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string CustomInfo { get; set; }

        public bool Active { get; set; }

        public DateTime? LastLogin { get; set; }
		public byte[] Salt { get; set; }
        public string PasswordHash { get; set; }
        public string EmailToken { get; set; }		// sent in email for pw reset, email confirm, etc

        // get a user from the info stored in the JWT
        public static AppUser FromIdentity(ClaimsPrincipal p)
        {
	        return new AppUser
	        {
		        // see Jwt.cs for more info
		        // these should match with Jwt.cs
		        Name = p.Claims.Single(c => c.Type == ClaimTypes.Name).Value,
		        Email = p.Claims.Single(c => c.Type == ClaimTypes.Email).Value,
		        Id = int.Parse(p.Claims.Single(c => c.Type == ClaimTypes.Sid).Value),
		        Role = p.Claims.Single(c => c.Type == ClaimTypes.Role).Value,
		        // example of a custom claim
		        CustomInfo = p.Claims.SingleOrDefault(c => c.Type == "CustomInfo")?.Value,
	        };
        }
	}
}
