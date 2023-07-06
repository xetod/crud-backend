using System.Linq.Expressions;

namespace RepositoryService.Data.Core.Specifications;

public sealed class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        _right = right;
        _left = left;
    }

    public override Expression<Func<T, bool>> ToBoolExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToBoolExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToBoolExpression();

        var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);

        return (Expression<Func<T, Boolean>>)Expression.Lambda(Expression.AndAlso(leftExpression.Body, invokedExpression), leftExpression.Parameters);
    }

    // private readonly Specification<T> _left;
    // private readonly Specification<T> _right;
    //
    // public AndSpecification(Specification<T> left, Specification<T> right)
    // {
    //     _right = right;
    //     _left = left;
    // }
    //
    // public override Expression<Func<T, bool>> ToBoolExpression()
    // {
    //     var leftExpression = _left.ToBoolExpression();
    //     var rightExpression = _right.ToBoolExpression();
    //
    //     var andExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
    //
    //     return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters.Single());
    // }
}