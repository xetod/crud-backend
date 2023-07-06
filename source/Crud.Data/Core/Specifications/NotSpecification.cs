using System.Linq.Expressions;

namespace RepositoryService.Data.Core.Specifications;

public sealed class NotSpecification<T> : Specification<T>
{
    private readonly Specification<T> _specification;

    public NotSpecification(Specification<T> specification)
    {
        _specification = specification;
    }

    public override Expression<Func<T, bool>> ToBoolExpression()
    {
        var expression = _specification.ToBoolExpression();
        var notExpression = Expression.Not(expression.Body);

        return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());
    }

    //private readonly Specification<T> _specification;

    //public NotSpecification(Specification<T> specification)
    //{
    //    _specification = specification;
    //}

    //public override Expression<Func<T, bool>> ToBoolExpression()
    //{
    //    var expression = _specification.ToBoolExpression();

    //    var notExpression = Expression.Not(expression.Body);

    //    return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());
    //}
}