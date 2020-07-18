using Microsoft.AspNetCore.Authorization;
using VueCoreJwt.Models;

namespace VueCoreJwt.Controllers
{
	[Authorize]
	public class SecureApiController : BaseApiController
	{
		private AppUser _currentUser;

		// CurrentUser is hydrated from the information in the JWT token
		public AppUser CurrentUser
		{
			get
			{
				if (_currentUser == null)
				{
					_currentUser = AppUser.FromIdentity(User);
				}
				return _currentUser;
			}
		}
	}
}
