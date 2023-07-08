using Crud.Data.Core.PagedLists;
using Crud.Data.Core.Specifications;
using Crud.Data.DbContexts;
using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Repositories.Customers;

/// <summary>
/// Represents a repository implementation for customer entities.
/// </summary>
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRepository"/> class with the specified CrudDbContext.
    /// </summary>
    /// <param name="context">The CrudDbContext used by the repository.</param>
    public CustomerRepository(CrudDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves a paged list of customers based on the specified specification, current page, and page size.
    /// </summary>
    /// <param name="specification">The specification to filter and sort the customers.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="pageSize">The number of customers to include in each page.</param>
    /// <returns>A paged list of customers based on the specified criteria.</returns>
    public async Task<PagedList<Customer>> GetCustomersWithPagination(Specification<Customer> specification, int currentPage = 0, int pageSize = 0)
    {
        var query = CrudDbContext
            .Customer
            .Include(customer => customer.Sales)
            .ThenInclude(sale => sale.Product)
            .Select(customer => new Customer
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Sales = customer.Sales.Select(sale => new Sale
                {
                    Product = new Product
                    {
                        Name = sale.Product.Name
                    }
                }).ToList()
            })
            .AsQueryable();

        query = SpecificationEvaluator<Customer>.GetQuery(query, specification);

        var customers = await PagedList<Customer>.CreateAsync(query, currentPage, pageSize);

        return customers;
    }

    /// <summary>
    /// Retrieves a list of all customers.
    /// </summary>
    /// <returns>A list of all customers.</returns>
    public async Task<List<Customer>> GetCustomers()
    {
        return await CrudDbContext
            .Customer
            .Include(customer => customer.Sales)
            .ThenInclude(sale => sale.Product)
            .Select(customer => new Customer
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Sales = customer.Sales.Select(sale => new Sale
                {
                    Product = new Product
                    {
                        Name = sale.Product.Name
                    }
                }).ToList()
            })
            .ToListAsync();
    }

    /// <summary>
    /// Gets the CrudDbContext as the underlying CrudDbContext instance.
    /// </summary>
    private CrudDbContext CrudDbContext => Context as CrudDbContext;
}
