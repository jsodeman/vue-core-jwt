using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueCoreJwt.App;
using VueCoreJwt.Data;
using VueCoreJwt.Interfaces;
using VueCoreJwt.Models;

namespace VueCoreJwt.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		private readonly AppConfig config;
		private readonly IDatabase db;
		private readonly IEmailService emailService;

		public RegisterController(AppConfig config, IDatabase db, IEmailService emailService)
		{
			this.config = config;
			this.db = db;
			this.emailService = emailService;
		}

		[HttpPost]
		public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
		{

			var existingUser = db.GetUserByEmail(request.Email);

			if (existingUser != null)
			{
				return BadRequest("A User with this address already exists");
			}

			var salt = Security.GenerateSalt();
			var hash = Security.HashPassword(request.Password, salt);
			var token = Security.GeneratePasswordResetIdentifier();

			var user = new AppUser
			{
				Email = request.Email.Trim(),
				Name = request.Name.Trim(),
				LastLogin = null,
				Role = "Normal",
				Salt = salt,
				PasswordHash = hash,
				EmailToken = token,
				Active = !config.ValidateEmail
			};

			user.Id = db.AddUser(user);

			if (!config.ValidateEmail)
			{
				var response = new AuthResponse(config, user, true);

				HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", response.Token, new CookieOptions { MaxAge = TimeSpan.FromMinutes(60) });

				return response;
			}

			await emailService.Register(user);

			return Ok();
		}

		// this is only used if validating email
		[HttpPost("confirm")]
		public ActionResult<AuthResponse> Confirm(ConfirmRequest request)
		{
			var user = db.GetUserByToken(request.EmailToken);

			if (user == null)
			{
				return new NotFoundObjectResult("Invalid confirmation link");
			}

			if (user.Active)
			{
				return new BadRequestObjectResult("Account already confirmed");
			}

			user.Active = true;
			user.LastLogin = DateTime.UtcNow;
			user.EmailToken = null;

			db.UpdateUser(user);

			var response = new AuthResponse(config, user, true);

			HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", response.Token, new CookieOptions { MaxAge = TimeSpan.FromMinutes(60) });

			return response;
		}
	}

	public class RegisterRequest
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class ConfirmRequest
	{
		public string EmailToken { get; set; }
	}
}
