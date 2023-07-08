using System.Linq.Expressions;

namespace Crud.Data.Core.Specifications;

/// <summary>
/// Represents an abstract base class for creating specifications that define query and filtering criteria for entities of type T.
/// Specifications can be combined using logical AND, OR, and NOT operators, and can include sorting configurations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public abstract class Specification<T>
{
    public static readonly Specification<T> All = new IdentitySpecification<T>();

    public static Specification<T> operator &(Specification<T> lhs, Specification<T> rhs) => lhs.And(rhs);

    public static Specification<T> operator |(Specification<T> lhs, Specification<T> rhs) => lhs.Or(rhs);

    public static Specification<T> operator !(Specification<T> spec) => spec.Not();

    /// <summary>
    /// Gets the list of sorting configurations for the specification.
    /// </summary>
    public List<Sort<T>> Sorts { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Specification{T}"/> class.
    /// </summary>
    protected Specification()
    {
        Sorts = new List<Sort<T>>();
    }

    /// <summary>
    /// Combines the current specification with another specification using the logical AND operator.
    /// </summary>
    /// <param name="specification">The specification to combine with.</param>
    /// <returns>The combined specification.</returns>
    public Specification<T> And(Specification<T> specification)
    {
        if (this == All)
            return specification;
        if (specification == All)
            return this;

        return new AndSpecification<T>(this, specification);
    }

    /// <summary>
    /// Combines the current specification with another specification using the logical OR operator.
    /// </summary>
    /// <param name="specification">The specification to combine with.</param>
    /// <returns>The combined specification.</returns>
    public Specification<T> Or(Specification<T> specification)
    {
        if (this == All || specification == All)
            return All;

        return new OrSpecification<T>(this, specification);
    }

    /// <summary>
    /// Negates the current specification using the logical NOT operator.
    /// </summary>
    /// <returns>The negated specification.</returns>
    public Specification<T> Not()
    {
        return new NotSpecification<T>(this);
    }

    /// <summary>
    /// Specifies that the collection should be sorted in ascending order based on the specified specification.
    /// </summary>
    /// <param name="specification">The specification for sorting.</param>
    /// <returns>The current specification with the ascending sort configuration.</returns>
    public Specification<T> SortAscending(Specification<T> specification)
    {
        Sorts.Add(new Sort<T>
        {
            Specification = new SortSpecification<T>(specification),
            Ascending = true
        });

        return this;
    }

    /// <summary>
    /// Specifies that the collection should be sorted in descending order based on the specified specification.
    /// </summary>
    /// <param name="specification">The specification for sorting.</param>
    /// <returns>The current specification with the descending sort configuration.</returns>
    public Specification<T> SortDescending(Specification<T> specification)
    {
        Sorts.Add(new Sort<T>
        {
            Specification = new SortSpecification<T>(specification),
            Ascending = false
        });

        return this;
    }

    /// <summary>
    /// Converts the specification into a boolean expression.
    /// </summary>
    /// <returns>The boolean expression representing the specification.</returns>
    public virtual Expression<Func<T, bool>> ToBoolExpression()
    {
        return x => true;
    }

    /// <summary>
    /// Converts the specification into an object expression.
    /// </summary>
    /// <returns>The object expression representing the specification.</returns>
    public virtual Expression<Func<T, object>> ToObjectExpression()
    {
        return x => default;
    }
}
