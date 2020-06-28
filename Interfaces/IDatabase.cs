using VueCoreJwt.Models;

namespace VueCoreJwt.Data
{
	public interface IDatabase
	{
		int AddUser(AppUser user);
		AppUser GetUserById(int id);
		AppUser GetUserByEmail(string email);
		AppUser GetUserByToken(string token);
		void UpdateUser(AppUser user);
	}
}
