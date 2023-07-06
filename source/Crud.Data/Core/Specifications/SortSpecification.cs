using System.Linq.Expressions;

namespace RepositoryService.Data.Core.Specifications;

public sealed class SortSpecification<T> : Specification<T>
{
    private readonly Specification<T> _specification;

    public SortSpecification(Specification<T> specification)
    {
        _specification = specification;
    }

    public override Expression<Func<T, object>> ToObjectExpression()
    {
        var expression = _specification.ToObjectExpression();

        var notExpression = expression.Body;

        return Expression.Lambda<Func<T, object>>(notExpression, expression.Parameters.Single());
    }
}