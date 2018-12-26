using MongoDB.Driver;
using Inflector;

namespace Common.Database
{
	public abstract class MongoRepositoryBase<TEntity>
	{
		protected readonly IMongoCollection<TEntity> MongoCollection;

		protected MongoRepositoryBase(string url)
		{
			var mongoUrl = new MongoUrl(url);
			MongoCollection = new MongoClient(mongoUrl)
				.GetDatabase(mongoUrl.DatabaseName)
				.GetCollection<TEntity>(typeof(TEntity).Name.Pluralize());
		}
	}
}
