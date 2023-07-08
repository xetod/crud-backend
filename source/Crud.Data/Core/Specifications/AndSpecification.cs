using System.Linq.Expressions;

namespace Crud.Data.Core.Specifications;

/// <summary>
/// Represents a composite specification that combines two specifications using a logical AND operator.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    /// <summary>
    /// Initializes a new instance of the <see cref="AndSpecification{T}"/> class.
    /// </summary>
    /// <param name="left">The left specification to combine.</param>
    /// <param name="right">The right specification to combine.</param>
    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left ?? throw new ArgumentNullException(nameof(left));
        _right = right ?? throw new ArgumentNullException(nameof(right));
    }

    /// <summary>
    /// Converts the composite specification into a boolean expression.
    /// </summary>
    /// <returns>The boolean expression representing the composite specification.</returns>
    public override Expression<Func<T, bool>> ToBoolExpression()
    {
        var leftExpression = _left.ToBoolExpression();
        var rightExpression = _right.ToBoolExpression();

        // Invoke the right expression using the parameters of the left expression
        var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);

        // Combine the left and invoked right expressions using the logical AND operator
        var andExpression = Expression.AndAlso(leftExpression.Body, invokedExpression);

        // Create a new lambda expression with the combined expression and the parameters of the left expression
        return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters);
    }
}
