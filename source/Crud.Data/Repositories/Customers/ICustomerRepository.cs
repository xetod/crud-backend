using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;

namespace Crud.Data.Repositories.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<List<Customer>> GetCustomers();
}