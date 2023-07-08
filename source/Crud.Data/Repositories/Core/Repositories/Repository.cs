﻿using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Repositories.Core.Repositories;

/// <summary>
/// Represents a generic repository implementation that provides basic CRUD operations for entities of type TEntity.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
{
    /// <summary>
    /// The DbContext used by the repository.
    /// </summary>
    protected readonly DbContext Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class with the specified DbContext.
    /// </summary>
    /// <param name="context">The DbContext used by the repository.</param>
    public Repository(DbContext context)
    {
        Context = context;
    }

    /// <summary>
    /// Deletes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public void Delete(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    /// <summary>
    /// Asynchronously adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public virtual async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    /// <summary>
    /// Disposes the repository and the associated DbContext.
    /// </summary>
    public void Dispose()
    {
        Context.Dispose();
    }
}
