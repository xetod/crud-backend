namespace Crud.Data.Core.Specifications;

/// <summary>
/// Represents a helper class for evaluating and applying specifications to an input query for entities of type TEntity.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class SpecificationEvaluator<TEntity> where TEntity : class
{
    /// <summary>
    /// Gets the query with the applied specifications based on the input query and the provided specification.
    /// </summary>
    /// <param name="inputQuery">The input query to apply the specifications to.</param>
    /// <param name="specification">The specification to be applied to the query.</param>
    /// <returns>The query with the applied specifications.</returns>
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, Specification<TEntity> specification)
    {
        // Copy the input query
        var query = inputQuery;

        // Apply the specification's boolean expression as a filter to the query
        query = query.Where(specification.ToBoolExpression());

        // Apply sorting if specified
        if (specification.Sorts.Any() && specification.Sorts[0].Ascending)
            query = query.OrderBy(specification.Sorts[0].Specification.ToObjectExpression());
        else if (specification.Sorts.Any() && !specification.Sorts[0].Ascending)
            query = query.OrderByDescending(specification.Sorts[0].Specification.ToObjectExpression());

        // Convert the query to an ordered query to support subsequent sorting
        var sorted = (IOrderedQueryable<TEntity>)query;

        // Apply additional sorting configurations, if any
        foreach (var sort in specification.Sorts.Skip(1))
        {
            sorted = sort.Ascending
                ? sorted.ThenBy(sort.Specification.ToObjectExpression())
                : sorted.ThenByDescending(sort.Specification.ToObjectExpression());
        }

        return sorted.AsQueryable<TEntity>();
    }
}
