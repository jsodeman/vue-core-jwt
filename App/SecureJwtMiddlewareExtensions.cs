using Microsoft.AspNetCore.Builder;

namespace VueCoreJwt.App
{
	public static class SecureJwtMiddlewareExtensions
	{
		public static IApplicationBuilder UseSecureJwt(this IApplicationBuilder builder) => builder.UseMiddleware<SecureJwtMiddleware>();
	}
}