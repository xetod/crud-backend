using System.Linq.Expressions;
using Crud.Domain.Entities;
using RepositoryService.Data.Core.Specifications;

namespace Crud.Application.Services.Customers.GetCustomers.Specifications;

public sealed class CustomerSortByNameSpecification : Specification<Customer>
{
    public override Expression<Func<Customer, object>> ToObjectExpression()
    {
        return customer => customer.Name;
    }
}