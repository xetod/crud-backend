using System.Linq.Expressions;
using Crud.Data.Core.Specifications;
using Crud.Domain.Entities.Customers;

namespace Crud.Application.Services.Customers.GetCustomers.Specifications;

/// <summary>
/// Represents a specification to filter customers by name.
/// </summary>
public sealed class CustomerByNameSpecification : Specification<Customer>
{
    private readonly string _customerName;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerByNameSpecification"/> class with the specified customer name.
    /// </summary>
    /// <param name="customerName">The customer name to filter by.</param>
    public CustomerByNameSpecification(string customerName)
    {
        _customerName = customerName;
    }

    /// <summary>
    /// Converts the specification to a boolean expression that can be used to filter customers by name.
    /// </summary>
    /// <returns>The boolean expression representing the specification.</returns>
    public override Expression<Func<Customer, bool>> ToBoolExpression()
    {
        return customer => (customer.FirstName + " " + customer.LastName).Contains(_customerName ?? string.Empty);
    }
}
