using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Api.Controllers
{
	[ApiController,
	 //Consumes("application/json", "application/xml"),
	 //Produces("application/json", "application/xml"),
	 ProducesResponseType(StatusCodes.Status401Unauthorized),
	 ProducesResponseType(StatusCodes.Status415UnsupportedMediaType),
	 ProducesResponseType(StatusCodes.Status500InternalServerError),
	 ProducesResponseType(StatusCodes.Status502BadGateway),
	 ProducesResponseType(StatusCodes.Status503ServiceUnavailable),
	 ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
	public abstract class ApiControllerBase : Controller
	{
		/// <summary>
		/// IP Address of the remote connected party
		/// </summary>
		protected IPAddress IpAddress =>
			HttpContext.Connection.RemoteIpAddress ?? new IPAddress(127001);

		/// <summary>
		/// Shorthand to get to the cancellation token
		/// </summary>
		protected CancellationToken CancellationToken =>
			HttpContext.RequestAborted;
	}
}
