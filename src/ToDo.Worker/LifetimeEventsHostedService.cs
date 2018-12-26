using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace ToDo.Worker
{
	internal class LifetimeEventsHostedService : IHostedService
	{
		private readonly ILogger<LifetimeEventsHostedService> _logger;
		private readonly IApplicationLifetime _appLifetime;
		private IEndpointInstance _endpoint;

		public LifetimeEventsHostedService(
			ILogger<LifetimeEventsHostedService> logger,
			IApplicationLifetime appLifetime)
		{
			_logger = logger;
			_appLifetime = appLifetime;
		}

		// Perform bootstrap items here
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_appLifetime.ApplicationStarted.Register(OnStarted);
			_appLifetime.ApplicationStopping.Register(OnStopping);
			_appLifetime.ApplicationStopped.Register(OnStopped);

			try
			{
				var endpointConfiguration =
					new EndpointConfiguration("ToDo.Worker");
				endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

				endpointConfiguration
					.DefineCriticalErrorAction(OnCriticalError);
				endpointConfiguration.UseTransport<LearningTransport>();
				endpointConfiguration.UsePersistence<LearningPersistence>();
				endpointConfiguration.EnableInstallers();
				_endpoint = await Endpoint.Start(endpointConfiguration);
			}
			catch (Exception ex)
			{
				_logger.LogCritical("Failed to start.", ex);
			}
		}

		// Perform cleanup items here
		public async Task StopAsync(CancellationToken cancellationToken)
		{
			try
			{
				if (_endpoint != null)
					await _endpoint.Stop();
			}
			catch (Exception ex)
			{
				_logger.LogCritical("Failed to stop correctly.", ex);
			}
		}

		// Perform post-startup activities here
		private void OnStarted() =>
			_logger.LogInformation("OnStarted has been called.");

		// Perform on-stopping activities here
		private void OnStopping() =>
			_logger.LogInformation("OnStopping has been called.");

		// Perform post-stopped activities here
		private void OnStopped() =>
			_logger.LogInformation("OnStopped has been called.");

		private async Task OnCriticalError(ICriticalErrorContext context)
		{
			// TODO: decide if stopping the endpoint and exiting the process is the best response to a critical error
			// https://docs.particular.net/nservicebus/hosting/critical-errors
			// and consider setting up service recovery
			// https://docs.particular.net/nservicebus/hosting/windows-service#installation-restart-recovery
			try
			{
				await context.Stop();
			}
			finally
			{
				_logger.LogCritical(
					$"Critical error, shutting down: {context.Error}",
					context.Exception);
			}
		}
	}
}
