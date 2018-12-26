using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Common.Database
{
	public class MongoQueryRepository<T> : MongoRepositoryBase<T>,
		IQueryRepository<T>
	{
		protected readonly IMongoQueryable<T> MongoQueryable;

		// ReSharper disable once UnusedMember.Global
		public MongoQueryRepository(IOptions<MongoSettings> settings) :
			this(settings.Value.ToUrl(MongoConnectionType.Query)) { }

		// This method should only be visible to derived classes
		protected MongoQueryRepository(string url) :
			base(url) => MongoQueryable = MongoCollection.AsQueryable();

		public async Task<int> CountAsync(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default) =>
			await MongoQueryable.CountAsync(expression, cancellationToken);

		public async Task<T> FirstAsync(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default) =>
			await MongoQueryable.FirstAsync(expression, cancellationToken);

		public async Task<T> FirstOrDefaultAsync(
			Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default) =>
			await MongoQueryable.FirstOrDefaultAsync(expression,
				cancellationToken);

		public async Task<T> SingleAsync(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default) =>
			await MongoQueryable.SingleAsync(expression, cancellationToken);

		public async Task<T> SingleOrDefaultAsync(
			Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default) =>
			await MongoQueryable.SingleOrDefaultAsync(expression,
				cancellationToken);

		public async Task<T> SingleAsync(
			CancellationToken cancellationToken = default) =>
			await MongoQueryable.SingleAsync(cancellationToken);

		public async Task<T>
			SingleOrDefaultAsync(
				CancellationToken cancellationToken = default) =>
			await MongoQueryable.SingleOrDefaultAsync(cancellationToken);

		public async Task<IList<T>> ToListAsync(
			Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default) =>
			await MongoQueryable.Where(expression)
				.ToListAsync(cancellationToken);

		public async Task<IList<T>> ToListAsync(
			CancellationToken cancellationToken = default) =>
			await MongoQueryable.ToListAsync(cancellationToken);

		public async Task<IList<T>> ToListAsync(
			Expression<Func<T, bool>> expression, int limit,
			CancellationToken cancellationToken = default) =>
			await MongoQueryable.Where(expression).Take(limit)
				.ToListAsync(cancellationToken);
	}

}
