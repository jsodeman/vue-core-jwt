using System.Threading.Tasks;
using VueCoreJwt.Models;

// TODO: customize for your needs
namespace VueCoreJwt.Interfaces
{
	public interface IEmailService
	{
		Task Register(AppUser user);
		Task Reset(AppUser user);
	}
}
