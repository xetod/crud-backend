using System.Linq.Expressions;

namespace Crud.Data.Core.Specifications;

/// <summary>
/// Represents an identity specification that always evaluates to true.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class IdentitySpecification<T> : Specification<T>
{
    /// <summary>
    /// Converts the identity specification into a boolean expression that always evaluates to true.
    /// </summary>
    /// <returns>The boolean expression representing the identity specification.</returns>
    public override Expression<Func<T, bool>> ToBoolExpression()
    {
        // Create an expression that always evaluates to true
        Expression<Func<T, bool>> trueExpression = x => true;

        // Return the true expression
        return trueExpression;
    }
}
