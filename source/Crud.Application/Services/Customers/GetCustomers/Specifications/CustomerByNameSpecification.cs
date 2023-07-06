using System.Linq;
using System.Linq.Expressions;
using Crud.Domain.Entities;
using RepositoryService.Data.Core.Specifications;

namespace Crud.Application.Services.Customers.GetCustomers.Specifications;

public sealed class CustomerByNameSpecification : Specification<Customer>
{
    private readonly string _customerName;

    public CustomerByNameSpecification(string customerName)
    {
        _customerName = customerName;
    }

    public override Expression<Func<Customer, bool>> ToBoolExpression()
    {
        return customer => customer.Name.Contains(_customerName ?? string.Empty);
    }
}