using Crud.Data.Core.PagedLists;
using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;
using RepositoryService.Data.Core.Specifications;

namespace Crud.Data.Repositories.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<List<Customer>> GetCustomers();

    Task<PagedList<Customer>> GetCustomersWithPagination(Specification<Customer> specification, int currentPage = 0, int pageSize = 0);
}