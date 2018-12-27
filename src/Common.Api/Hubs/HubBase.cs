using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Common.Api.Hubs
{
	[Authorize(AuthenticationSchemes = AuthSchemes)]
	public abstract class HubBase : Hub
	{
		protected const string AuthSchemes =
			CookieAuthenticationDefaults.AuthenticationScheme + "," +
			JwtBearerDefaults.AuthenticationScheme;
	}
}
