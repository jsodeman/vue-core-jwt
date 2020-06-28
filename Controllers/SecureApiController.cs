using Microsoft.AspNetCore.Authorization;
using VueCoreJwt.Models;

namespace VueCoreJwt.Controllers
{
	[Authorize]
	public class SecureApiController : BaseApiController
	{
		private AppUser _currentUser;

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
