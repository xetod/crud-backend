using System.Linq.Expressions;

namespace Crud.Data.Repositories.Core.Repositories;

/// <summary>
/// Represents the interface for a generic repository that provides basic CRUD operations for entities of type TEntity.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Deletes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Asynchronously adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Retrieves the first entity that matches the specified condition asynchronously.
    /// </summary>
    /// <param name="match">The condition to match.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the condition, or the default value if no such entity is found.</returns>
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> match);
}
