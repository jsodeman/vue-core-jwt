using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueCoreJwt.App;
using VueCoreJwt.Data;
using VueCoreJwt.Models;

namespace VueCoreJwt.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : BaseApiController
	{
		private readonly AppConfig config;
		private readonly IDatabase db;

		public LoginController(AppConfig config, IDatabase db)
		{
			this.config = config;
			this.db = db;
		}

		[HttpPost]
		public ActionResult<AuthResponse> Post(LoginRequest request)
		{

			var user = db.GetUserByEmail(request.Email);
			if (user == null)
			{
				return new NotFoundObjectResult("Account not found");
			}

			// check the login against stored values
			var auth = user.PasswordHash == Security.HashPassword(request.Password, user.Salt);

			if (!auth)
			{
				return new BadRequestObjectResult("Incorrect password");
			}

			if (config.ValidateEmail && !user.Active)
			{
				// if using the email validated registration flow then reject users that haven't confirmed
				return new BadRequestObjectResult("Email has not been confirmed");
			}

			var firstLogin = !user.LastLogin.HasValue;

			user.LastLogin = DateTime.UtcNow;
			db.UpdateUser(user);

			var response = new AuthResponse (config, user, firstLogin);

			// set the auth cookie and send back the user info
			HttpContext.Response.Cookies.Append(config.CookieName, response.Token, new CookieOptions { MaxAge = TimeSpan.FromMinutes(60) });

			return response;
		}

		[HttpDelete]
		public ActionResult Logout()
		{
			HttpContext.Response.Cookies.Delete(config.CookieName);

			return Ok();
		}
	}

	public class LoginRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
