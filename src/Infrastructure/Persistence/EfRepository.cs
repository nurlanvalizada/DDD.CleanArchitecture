using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Entities;
using AppDomain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class EfRepository<TEntity, TPrimaryKey>(ApplicationDbContext dbContext) : IRepository<TEntity, TPrimaryKey>
    where TEntity : AggregateRoot<TPrimaryKey>
{
    private DbSet<TEntity> Table => dbContext.Set<TEntity>();

    public async Task<List<TEntity>> GetAllListAsync(CancellationToken cancellationToken = default)
    {
        return await GetAll().ToListAsync(cancellationToken: cancellationToken);
    }

    public Task<List<TEntity>> GetAllListIncludingAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = GetAll();
        includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return query.ToListAsync(cancellationToken: cancellationToken);
    }

    public Task<TEntity> GetFirstAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
    {
        return GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id), cancellationToken: cancellationToken);
    }

    public Task<TEntity> GetFirstIncludingAsync(TPrimaryKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = GetAll();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return query.FirstOrDefaultAsync(CreateEqualityExpressionForId(id), cancellationToken: cancellationToken);
    }

    public Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return GetAll().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<TEntity> GetFirstIncludingAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default,
                                                params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = GetAll();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return query.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return GetAll().Where(predicate).ToListAsync(cancellationToken);
    }

    public Task<List<TEntity>> FilterIncludingAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default,
                                                    params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = GetAll();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return query.Where(predicate).ToListAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return Table.AnyAsync(predicate);
    }

    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return Table.AllAsync(predicate);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await Table.CountAsync();
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return Table.CountAsync(predicate);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Table.AddAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Entry(entity).State = EntityState.Deleted;
    }

    public async Task DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        IEnumerable<TEntity> entities = Table.Where(predicate);

        foreach(var entity in entities)
        {
            dbContext.Entry(entity).State = EntityState.Deleted;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static void BindIncludeProperties(IQueryable<TEntity> query, IEnumerable<Expression<Func<TEntity, object>>> includeProperties)
    {
        includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    private static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
    {
        var lambdaParam = Expression.Parameter(typeof(TEntity));

        var lambdaBody = Expression.Equal(
            Expression.PropertyOrField(lambdaParam, "Id"),
            Expression.Constant(id, typeof(TPrimaryKey))
        );

        return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
    }

    private IQueryable<TEntity> GetAll()
    {
        IQueryable<TEntity> query = Table;
        return query;
    }

    private IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = GetAll();
        BindIncludeProperties(query, includeProperties);
        includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return query;
    }

    public void Dispose()
    {
        dbContext?.Dispose();
    }
}