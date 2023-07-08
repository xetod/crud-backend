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
}
