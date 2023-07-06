using System.Linq.Expressions;

namespace RepositoryService.Data.Core.Specifications;

public sealed class IdentitySpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToBoolExpression()
    {
        return x => true;
    }
}
