using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace VueCoreJwt.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientLogController : BaseApiController
	{
		// log data from clientside
		[HttpPost]
		public ActionResult Post(ClientLogRequest request)
		{
			Log.Error("[JS] {Error}", request.Error);

			return Ok();
		}
	}

	public class ClientLogRequest
	{
		public string Error { get; set; }
	}
}
