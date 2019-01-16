using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Common.Domain;

namespace Common.Database
{
	public interface IPartialObjectUpdater<TAudit, TEntity>
		where TAudit : AuditEntityBase<TEntity>
	{
		IPartialObjectUpdater<TAudit, TEntity> AddToSet<TItem>(
			Expression<Func<TAudit, TEntity, IEnumerable<TItem>>> field,
			TItem value);

		IPartialObjectUpdater<TAudit, TEntity> AddToSetEach<TItem>(
			Expression<Func<TAudit, TEntity, IEnumerable<TItem>>> field,
			IEnumerable<TItem> values);

		IPartialObjectUpdater<TAudit, TEntity> BitwiseAnd<TField>(
			Expression<Func<TAudit, TEntity, TField>> field,
			TField value);

		IPartialObjectUpdater<TAudit, TEntity> BitwiseOr<TField>(
			Expression<Func<TAudit, TEntity, TField>> field,
			TField value);

		IPartialObjectUpdater<TAudit, TEntity> BitwiseXor<TField>(
			Expression<Func<TAudit, TEntity, TField>> field,
			TField value);

		IPartialObjectUpdater<TAudit, TEntity> Inc<TField>(
			Expression<Func<TAudit, TEntity, TField>> field,
			TField value);

		IPartialObjectUpdater<TAudit, TEntity> Max<TField>(
			Expression<Func<TAudit, TEntity, TField>> field,
			TField value);

		IPartialObjectUpdater<TAudit, TEntity> Min<TField>(
			Expression<Func<TAudit, TEntity, TField>> field,
			TField value);

		IPartialObjectUpdater<TAudit, TEntity> Mul<TField>(
			Expression<Func<TAudit, TEntity, TField>> field,
			TField value);

		IPartialObjectUpdater<TAudit, TEntity> PopFirst<TField>(
			Expression<Func<TAudit, TEntity, TField>> field);

		IPartialObjectUpdater<TAudit, TEntity> PopLast<TField>(
			Expression<Func<TAudit, TEntity, TField>> field);

		IPartialObjectUpdater<TAudit, TEntity> Pull<TItem>(
			Expression<Func<TAudit, TEntity, IEnumerable<TItem>>> field,
			TItem value);

		IPartialObjectUpdater<TAudit, TEntity> PullAll<TItem>(
			Expression<Func<TAudit, TEntity, IEnumerable<TItem>>> field,
			IEnumerable<TItem> values);

		IPartialObjectUpdater<TAudit, TEntity> PullFilter<TItem>(
			Expression<Func<TAudit, TEntity, IEnumerable<TItem>>> field,
			Expression<Func<TItem, bool>> filter);

		IPartialObjectUpdater<TAudit, TEntity> Push<TItem>(
			Expression<Func<TAudit, TEntity, IEnumerable<TItem>>> field,
			TItem value);

		IPartialObjectUpdater<TAudit, TEntity> Set<TField>(
			Expression<Func<TAudit, TEntity, TField>> field,
			TField value);

		IPartialObjectUpdater<TAudit, TEntity> Unset<TField>(
			Expression<Func<TAudit, TEntity, TField>> field);

		Task<bool> SaveChangesAsync(TAudit audit,
			CancellationToken cancellationToken = default);

		bool HasChanges { get; }
	}
}
