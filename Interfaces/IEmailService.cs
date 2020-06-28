using System.Threading.Tasks;
using VueCoreJwt.Models;

namespace VueCoreJwt.Interfaces
{
	public interface IEmailService
	{
		Task Register(AppUser user);
		Task Reset(AppUser user);
	}
}
