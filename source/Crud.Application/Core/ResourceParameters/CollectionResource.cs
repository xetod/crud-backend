namespace Crud.Application.Core.ResourceParameters;

/// <summary>
/// Represents a collection of resources with pagination metadata.
/// </summary>
/// <typeparam name="TEntity">The type of the resource.</typeparam>
public class CollectionResource<TEntity>
{
    /// <summary>
    /// Gets or sets the pagination metadata for the collection.
    /// </summary>
    public PaginationMetadata Pagination { get; set; }

    /// <summary>
    /// Gets or sets the list of resources.
    /// </summary>
    public List<TEntity> Results { get; set; }
}