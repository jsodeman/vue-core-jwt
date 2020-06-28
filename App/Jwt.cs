using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VueCoreJwt.Models;

namespace VueCoreJwt.App
{
	public static class Jwt
	{
		public static string GenerateTokenForUser(AppConfig configuration, AppUser user)
		{
			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.JwtKey));

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Sid, user.Id.ToString()),
					new Claim(ClaimTypes.Role, user.Role),
					new Claim("CustomInfo", user.CustomInfo  ?? ""),

				}),
				Expires = DateTime.UtcNow.AddDays(configuration.JwtDays),
				SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
				Issuer = configuration.SiteUrl,
				Audience = configuration.SiteUrl,
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);

		}

		public static int UserIdFromJwt(string tokenString)
		{
			var jwtEncodedString = tokenString.StartsWith("Bearer") ? tokenString.Substring(7) : tokenString;
			var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);

			return int.Parse(token.Claims.Single(c => c.Type == ClaimTypes.Sid).Value);
		}
	}
}
