namespace RepositoryService.Data.Core.Specifications;

public class SpecificationEvaluator<TEntity> where TEntity : class
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, Specification<TEntity> specification)
    {
        var query = inputQuery;

        query = query.Where(specification.ToBoolExpression());

        if (specification.Sorts.Any() && specification.Sorts[0].Ascending)
            query = query.OrderBy(specification.Sorts[0].Specification.ToObjectExpression());
        else if (specification.Sorts.Any() && !specification.Sorts[0].Ascending)
            query = query.OrderByDescending(specification.Sorts[0].Specification.ToObjectExpression());

        var sorted = (IOrderedQueryable<TEntity>)query;

        foreach (var sort in specification.Sorts.Skip(1))
        {
            sorted = sort.Ascending
                ? sorted.ThenBy(sort.Specification.ToObjectExpression())
                : sorted.ThenByDescending(sort.Specification.ToObjectExpression());
        }

        return sorted.AsQueryable<TEntity>();
    }
}