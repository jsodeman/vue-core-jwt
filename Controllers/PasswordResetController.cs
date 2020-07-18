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
	public class PasswordResetController : BaseApiController
	{
		private readonly AppConfig config;
		private readonly IDatabase db;
		private readonly IEmailService emailService;

		public PasswordResetController(AppConfig config, IDatabase db, IEmailService emailService)
		{
			this.config = config;
			this.db = db;
			this.emailService = emailService;
		}

		[HttpPost]
		public async Task<ActionResult> ResetLink(PasswordResetLinkRequest request)
        {
            var user = db.GetUserByEmail(request.Email);
            if (user == null)
            {
                return new NotFoundObjectResult("Account not found");
            }

            // send an email with a link back to the app containing a verification token
            // TODO: if it matters you could store and check a time for the reset request to restrict the reset to a time window
            user.EmailToken = Security.GeneratePasswordResetIdentifier();
            db.UpdateUser(user);

            await emailService.Reset(user);

            return Ok();
        }

		[HttpPost("complete")]
		public ActionResult<AuthResponse> ResetComplete(PasswordResetRequest request)
		{
			var user = db.GetUserByToken(request.EmailToken);
            if (user == null)
            {
                return new NotFoundObjectResult("Invalid Reset Link");
            }
            if (!user.Active)
            {
                return new UnauthorizedObjectResult("Account disabled");
            }

            var firstLogin = !user.LastLogin.HasValue;

			var salt = Security.GenerateSalt();
            var hash = Security.HashPassword(request.NewPassword.Trim(), salt);

            user.EmailToken = null;
            user.Salt = salt;
            user.PasswordHash = hash;
            user.LastLogin = DateTime.UtcNow;
            db.UpdateUser(user);

            var response = new AuthResponse(config, user, firstLogin);

            HttpContext.Response.Cookies.Append(config.CookieName, response.Token, new CookieOptions { MaxAge = TimeSpan.FromMinutes(60) });
			// set the new cookie and return the user info
            return response;
        }
	}

	public class PasswordResetLinkRequest
	{
		public string Email { get; set; }
	}

	public class PasswordResetRequest
	{
		public string NewPassword { get; set; }
		public string EmailToken { get; set; }
	}
}
