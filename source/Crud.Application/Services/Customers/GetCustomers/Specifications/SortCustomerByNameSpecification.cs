using System.Linq.Expressions;
using Crud.Data.Core.Specifications;
using Crud.Domain.Entities;

namespace Crud.Application.Services.Customers.GetCustomers.Specifications;

/// <summary>
/// Represents a specification to sort customers by name.
/// </summary>
public sealed class SortCustomerByNameSpecification : Specification<Customer>
{
    /// <summary>
    /// Converts the specification to an object expression that can be used to sort customers by name.
    /// </summary>
    /// <returns>The object expression representing the specification.</returns>
    public override Expression<Func<Customer, object>> ToObjectExpression()
    {
        return customer => customer.LastName;
    }
}