using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Common.Domain;

namespace Common.Database
{
	public interface ICommandRepository<TAudit, TEntity> :
		IQueryRepository<TAudit>,
		IPartialObjectUpdater<TAudit, TEntity>
		where TAudit : AuditEntityBase<TEntity>
	{
		Task<long> DeleteAsync(Expression<Func<TAudit, bool>> expression,
			CancellationToken cancellationToken = default);

		Task<bool> DeleteAsync(TAudit entity,
			CancellationToken cancellationToken = default);

		Task<bool> InsertAsync(TAudit entity, IPAddress ipAddress,
			CancellationToken cancellationToken = default);

		Task<bool> UpdateAsync(TAudit entity, IPAddress ipAddress,
			CancellationToken cancellationToken = default);
	}
}
