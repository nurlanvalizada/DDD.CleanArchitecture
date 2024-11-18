using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Entities;

namespace AppDomain.Common.Interfaces;

public interface IRepository<TEntity, in TPrimaryKey> : IDisposable where TEntity : BaseEntity<TPrimaryKey>
{
    IQueryable<TEntity> GetAll();

    IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);

    Task<List<TEntity>> GetAllList();

    Task<List<TEntity>> GetAllListIncluding(params Expression<Func<TEntity, object>>[] includeProperties);

    ValueTask<TEntity> Find(TPrimaryKey id);

    Task<TEntity> GetFirst(TPrimaryKey id);

    Task<TEntity> GetFirstIncluding(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> GetFirstIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity> GetSingle(TPrimaryKey id);

    Task<TEntity> GetSingleIncluding(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> GetSingleIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

    IQueryable<TEntity> FindByIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<bool> Any(Expression<Func<TEntity, bool>> predicate);

    Task<bool> All(Expression<Func<TEntity, bool>> predicate);

    Task<int> Count();

    Task<int> Count(Expression<Func<TEntity, bool>> predicate);

    Task Add(TEntity entity);

    Task Update(TEntity entity);

    Task Delete(TEntity entity);

    Task DeleteWhere(Expression<Func<TEntity, bool>> predicate);

    Task Commit(CancellationToken cancellationToken);
}