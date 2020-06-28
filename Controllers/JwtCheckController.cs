using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueCoreJwt.Data;
using VueCoreJwt.Models;

namespace VueCoreJwt.Controllers
{
	// checks to see if the JWT token passed with the request is good and still valid
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

			if (user == null)
			{
				HttpContext.Response.Cookies.Delete(".AspNetCore.Application.Id");
				return new UnauthorizedResult();
			}

			if (config.ValidateEmail && !user.Active)
			{
				HttpContext.Response.Cookies.Delete(".AspNetCore.Application.Id");
				return new BadRequestObjectResult("Email has not been confirmed");
			}

			user.LastLogin = DateTime.UtcNow;
			db.UpdateUser(user);

			var response = new AuthResponse(config, user, false);

			HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", response.Token, new CookieOptions { MaxAge = TimeSpan.FromMinutes(60) });

			return response;
		}
	}
}
