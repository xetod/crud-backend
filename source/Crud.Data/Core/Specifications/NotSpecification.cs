using System.Linq.Expressions;

namespace Crud.Data.Core.Specifications;

/// <summary>
/// Represents a negation specification that negates the result of another specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class NotSpecification<T> : Specification<T>
{
    private readonly Specification<T> _specification;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotSpecification{T}"/> class.
    /// </summary>
    /// <param name="specification">The specification to negate.</param>
    public NotSpecification(Specification<T> specification)
    {
        _specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    /// <summary>
    /// Converts the negation specification into a boolean expression.
    /// </summary>
    /// <returns>The boolean expression representing the negation specification.</returns>
    public override Expression<Func<T, bool>> ToBoolExpression()
    {
        // Get the expression of the inner specification
        var expression = _specification.ToBoolExpression();

        // Negate the inner expression using the NOT operator
        var notExpression = Expression.Not(expression.Body);

        // Create a new lambda expression with the negated expression and the parameter of the inner expression
        return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());
    }
}
