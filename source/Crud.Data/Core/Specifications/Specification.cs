using System.Linq.Expressions;

namespace RepositoryService.Data.Core.Specifications;

public abstract class Specification<T> 
{
    public static readonly Specification<T> All = new IdentitySpecification<T>();

    public static Specification<T> operator &(Specification<T> lhs, Specification<T> rhs) => lhs.And(rhs);

    public static Specification<T> operator |(Specification<T> lhs, Specification<T> rhs) => lhs.Or(rhs);

    public static Specification<T> operator !(Specification<T> spec) => spec.Not();


    public List<Sort<T>> Sorts { get; }

    protected Specification()
    {
        Sorts = new List<Sort<T>>();
    }

    public Specification<T> And(Specification<T> specification)
    {
        if (this == All)
            return specification;
        if (specification == All)
            return this;

        return new AndSpecification<T>(this, specification);
    }

    public Specification<T> Or(Specification<T> specification)
    {
        if (this == All || specification == All)
            return All;

        return new OrSpecification<T>(this, specification);
    }

    public Specification<T> Not()
    {
        return new NotSpecification<T>(this);
    }

    public Specification<T> SortAscending(Specification<T> specification)
    {
        Sorts.Add(new Sort<T>
        {
            Specification = new SortSpecification<T>(specification),
            Ascending = true
        });

        return this;
    }

    public Specification<T> SortDescending(Specification<T> specification)
    {
        Sorts.Add(new Sort<T>
        {
            Specification = new SortSpecification<T>(specification),
            Ascending = false
        });

        return this;
    }

    public virtual Expression<Func<T, bool>> ToBoolExpression()
    {
        return x => true;
    }

    public virtual Expression<Func<T, object>> ToObjectExpression()
    {
        return x => default;
    }
}