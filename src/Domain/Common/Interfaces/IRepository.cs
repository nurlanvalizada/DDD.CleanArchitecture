using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Entities;

namespace AppDomain.Common.Interfaces;

public interface IRepository<TEntity, in TPrimaryKey> : IDisposable where TEntity : AggregateRoot<TPrimaryKey>
{
    Task<List<TEntity>> GetAllListAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllListIncludingAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity> GetFirstAsync(TPrimaryKey id, CancellationToken cancellationToken = default);

    Task<TEntity> GetFirstIncludingAsync(TPrimaryKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default);

    Task<TEntity> GetFirstIncludingAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);
    
    Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default);

    Task<List<TEntity>> FilterIncludingAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default);

    Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity,CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity,CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity,CancellationToken cancellationToken = default);

    Task DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken);
}