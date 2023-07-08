using Crud.Data.Core.PagedLists;
using Crud.Data.Core.Specifications;
using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;

namespace Crud.Data.Repositories.Customers;

/// <summary>
/// Represents a repository interface for customer entities.
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    /// <summary>
    /// Retrieves a list of all customers.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of all customers.</returns>
    Task<List<Customer>> GetCustomers();

    /// <summary>
    /// Retrieves a paged list of customers based on the specified specification, current page, and page size.
    /// </summary>
    /// <param name="specification">The specification to filter and sort the customers.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="pageSize">The number of customers to include in each page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of customers based on the specified criteria.</returns>
    Task<PagedList<Customer>> GetCustomersWithPagination(Specification<Customer> specification, int currentPage = 0, int pageSize = 0);
}
