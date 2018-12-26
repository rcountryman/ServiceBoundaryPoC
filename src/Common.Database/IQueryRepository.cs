using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Database
{
	public interface IQueryRepository<T>
	{
		Task<int> CountAsync(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default);

		Task<T> FirstAsync(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default);

		Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default);

		Task<T> SingleAsync(CancellationToken cancellationToken = default);

		Task<T> SingleAsync(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default);

		Task<T> SingleOrDefaultAsync(
			CancellationToken cancellationToken = default);

		Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default);

		Task<IList<T>> ToListAsync(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default);

		Task<IList<T>> ToListAsync(
			CancellationToken cancellationToken = default);

		Task<IList<T>> ToListAsync(Expression<Func<T, bool>> expression,
			int limit, CancellationToken cancellationToken = default);
	}
}
