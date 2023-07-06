namespace Crud.Application.Core.ResourceParameters;

public class CollectionResource<TEntity>
{
    public PaginationMetadata Pagination { get; set; }

    public List<TEntity> Results { get; set; }
}