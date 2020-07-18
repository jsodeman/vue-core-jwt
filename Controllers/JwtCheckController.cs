using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueCoreJwt.Data;
using VueCoreJwt.Models;

namespace VueCoreJwt.Controllers
{
	// checks to see if the JWT token passed with the request is good and still valid
	// sets a fresh cookie and returns the latest user info
	[Route("api/[controller]")]
	[ApiController]
	public class JwtCheckController : SecureApiController
	{
		private readonly IDatabase db;
		private readonly AppConfig config;

		public JwtCheckController(IDatabase db, AppConfig config)
		{
			this.db = db;
			this.config = config;
		}

		[HttpGet]
		public ActionResult<AuthResponse> Get()
		{
			var user = db.GetUserById(CurrentUser.Id);

			// TODO: customize for your needs
			// if you wanted to check a token invalidation list then you would do it here

			if (user == null)
			{
				// if the user isn't found then clear the cookie
				HttpContext.Response.Cookies.Delete(config.CookieName);
				return new UnauthorizedResult();
			}

			if (config.ValidateEmail && !user.Active)
			{
				// if using the email validated registration flow then reject users that haven't confirmed
				HttpContext.Response.Cookies.Delete(config.CookieName);
				return new BadRequestObjectResult("Email has not been confirmed");
			}

			user.LastLogin = DateTime.UtcNow;
			db.UpdateUser(user);

			var response = new AuthResponse(config, user, false);

			// set a fresh cookie
			HttpContext.Response.Cookies.Append(config.CookieName, response.Token, new CookieOptions { MaxAge = TimeSpan.FromMinutes(60) });

			// return the latest user info
			return response;
		}
	}
}
