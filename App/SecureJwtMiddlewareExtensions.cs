using Microsoft.AspNetCore.Builder;

namespace VueCoreJwt.App
{
	// this is used to help register the JWT middleware
	public static class SecureJwtMiddlewareExtensions
	{
		public static IApplicationBuilder UseSecureJwt(this IApplicationBuilder builder) => builder.UseMiddleware<SecureJwtMiddleware>();
	}
}