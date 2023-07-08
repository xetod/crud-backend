using System.Linq.Expressions;

namespace Crud.Data.Core.Specifications;

/// <summary>
/// Represents a specification that extracts an object expression for sorting from another specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class SortSpecification<T> : Specification<T>
{
    private readonly Specification<T> _specification;

    /// <summary>
    /// Initializes a new instance of the <see cref="SortSpecification{T}"/> class.
    /// </summary>
    /// <param name="specification">The specification to extract the object expression from.</param>
    public SortSpecification(Specification<T> specification)
    {
        _specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    /// <summary>
    /// Converts the sort specification into an object expression for sorting.
    /// </summary>
    /// <returns>The object expression representing the sort specification.</returns>
    public override Expression<Func<T, object>> ToObjectExpression()
    {
        // Get the object expression of the inner specification
        var expression = _specification.ToObjectExpression();

        // Use the body of the expression directly as the object expression for sorting
        var objectExpression = expression.Body;

        // Create a new lambda expression with the object expression and the parameter of the inner expression
        return Expression.Lambda<Func<T, object>>(objectExpression, expression.Parameters.Single());
    }
}
