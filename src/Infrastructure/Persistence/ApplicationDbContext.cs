using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.DomainEvents;
using AppDomain.Common.Entities;
using AppDomain.Entities;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions options, ICurrentUserService currentUserService, IMediator mediator) : DbContext(options)
{
    private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(ApplicationDbContext)
        .GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IAudited>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    SetCreationAuditProperties(entry);
                    break;
                case EntityState.Modified:
                    SetModificationAuditProperties(entry);
                    break;
                case EntityState.Deleted:
                    CancelDeletionForSoftDelete(entry);
                    SetDeletionAuditProperties(entry);
                    break;
            }
        }

        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // ignore events if no dispatcher provided
        if (mediator == null) 
            return result;

        // dispatch events only if save was successful
        var events = ChangeTracker.Entries<HaveDomainEvents>()
                                              .Select(e => e.Entity)
                                              .SelectMany(entity =>
                                              {
                                                  var domainEvents = entity.GetDomainEvents();
                                                  entity.ClearDomainEvents();
                                                  return domainEvents;
                                              }).ToArray();
        foreach(var @event in events)
        {
            await mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
        }

        return result;
    }

    public DbSet<ToDoTask> ToDoTasks { get; set; }

    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureGlobalFiltersMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, [modelBuilder, entityType]);
        }
    }

    #region Configure Global Filters

    protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType) where TEntity : class
    {
        if (ShouldFilterEntity<TEntity>(entityType))
        {
            var filterExpression = CreateFilterExpression<TEntity>();
            if (filterExpression != null)
            {
                modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
            }
        }
    }

    protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        return false;
    }

    protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>() where TEntity : class
    {
        Expression<Func<TEntity, bool>> expression = null;

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted;
            expression = softDeleteFilter;
        }

        return expression;
    }

    #endregion

    #region Configure Audit Properties

    protected virtual void SetCreationAuditProperties(EntityEntry entry)
    {
        if (!(entry.Entity is IHasCreationTime hasCreationTimeEntity)) return;

        if (hasCreationTimeEntity.CreatedDate == default)
        {
            hasCreationTimeEntity.CreatedDate = DateTime.Now;
        }

        if (!(entry.Entity is ICreationAudited creationAuditedEntity)) return;

        if (creationAuditedEntity.CreatedUserId != null)
        {
            //CreatedUserId is already set
            return;
        }

        creationAuditedEntity.CreatedUserId = currentUserService.UserId;
    }

    protected virtual void SetModificationAuditProperties(EntityEntry entry)
    {
        if (!(entry.Entity is IHasModificationTime hasModificationTimeEntity)) return;

        if (hasModificationTimeEntity.LastModifiedDate == default)
        {
            hasModificationTimeEntity.LastModifiedDate = DateTime.Now;
        }

        if (!(entry.Entity is IModificationAudited modificationAuditedEntity)) return;

        if (modificationAuditedEntity.LastModifiedUserId != null)
        {
            //LastModifiedUserId is already set
            return;
        }

        modificationAuditedEntity.LastModifiedUserId = currentUserService.UserId;
    }

    protected virtual void SetDeletionAuditProperties(EntityEntry entry)
    {

        if (!(entry.Entity is IHasDeletionTime hasDeletionTimeEntity)) return;

        if (hasDeletionTimeEntity.DeletedDate == default)
        {
            hasDeletionTimeEntity.DeletedDate = DateTime.Now;
        }

        if (!(entry.Entity is IDeletionAudited deletionAuditedEntity)) return;

        deletionAuditedEntity.DeletedUserId = currentUserService.UserId;
        deletionAuditedEntity.DeletedDate = DateTime.Now;
    }

    protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
    {
        if (!(entry.Entity is ISoftDelete))
        {
            return;
        }

        entry.Reload();
        entry.State = EntityState.Modified;
        ((ISoftDelete)entry.Entity).IsDeleted = true;
    }

    #endregion
}