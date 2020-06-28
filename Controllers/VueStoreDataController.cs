using System;
using Microsoft.AspNetCore.Mvc;
using VueCoreJwt.Models;

namespace VueCoreJwt.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VueStoreDataController : BaseApiController
	{
		private readonly AppConfig config;

		public VueStoreDataController(AppConfig config)
		{
			this.config = config;
		}

		[HttpGet]
		public ActionResult<VueStoreData> Get()
		{
			return new VueStoreData
			{
				SomeServiceApiKey = config.SomeServiceApiKey,
				ValidateEmail = config.ValidateEmail
			};
		}
	}

	// anything public you want passed from the server to the Vue app when it starts
	// lists, clientside keys, urls, config info, etc
	public class VueStoreData
	{
		public string SomeServiceApiKey { get; set; }		// Just a demo value passed to Vue
		public bool ValidateEmail { get; set; }
	}
}
