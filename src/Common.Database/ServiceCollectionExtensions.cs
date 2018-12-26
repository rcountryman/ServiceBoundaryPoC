using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Database
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMongoServices(
			this IServiceCollection services)
		{
			// Set the inflector data up once 
			Inflector.Inflector.SetDefaultCultureFunc = () =>
				Thread.CurrentThread.CurrentUICulture;

			// Map dependency injection info
			return services
				.AddScoped(typeof(IQueryRepository<>),
					typeof(MongoQueryRepository<>));
			/*
			.AddScoped(typeof(ICommandRepository<,>),
				typeof(MongoCommandRepository<,>));
			*/
		}
	}
}
