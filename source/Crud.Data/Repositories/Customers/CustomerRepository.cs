using Crud.Data.Core.PagedLists;
using Crud.Data.Core.Specifications;
using Crud.Data.DbContexts;
using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;
using Crud.Domain.Entities.Customers;
using Crud.Domain.Entities.Products;
using Crud.Domain.Entities.Sales;
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
            .AsNoTracking()
            .AsQueryable();

        query = SpecificationEvaluator<Customer>.GetQuery(query, specification);

        var customers = await PagedList<Customer>.CreateAsync(query, currentPage, pageSize);

        return customers;
    }

    /// <summary>
    /// Retrieves a customer by their ID from the database.
    /// </summary>
    /// <param name="customerId">The ID of the customer to retrieve.</param>
    /// <returns>The customer with the specified ID.</returns>
    public async Task<Customer> GetCustomer(int customerId)
    {
        var customer = await CrudDbContext
            .Customer
            .Include(customer => customer.Sales)
            .ThenInclude(sale => sale.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.CustomerId == customerId);

        return customer ?? new NullCustomer();
    }

    /// <summary>
    /// Updates a customer entity in the database based on the provided updated customer and the existing customer from the database.
    /// </summary>
    /// <param name="customerToUpdate">The updated customer entity.</param>
    /// <param name="customerFromDb">The existing customer entity from the database.</param>
    public void Update(Customer customerToUpdate, Customer customerFromDb)
    {
        // Check if either customerToUpdate or customerFromDb is null, and return if true
        if (customerFromDb == null || customerToUpdate == null)
            return;

        // Check if the CustomerId of customerToUpdate matches the CustomerId of customerFromDb, and return if not matching
        if (customerToUpdate.CustomerId != customerFromDb.CustomerId)
            return;

        // Check if either CustomerId is less than or equal to 0, and return if true
        if (customerToUpdate.CustomerId <= 0 || customerFromDb.CustomerId <= 0)
            return;

        // Get the entity entry for the customerFromDb
        var dbEntry = Context.Entry(customerFromDb);

        // Update the properties of customerFromDb with the values from customerToUpdate
        dbEntry.CurrentValues.SetValues(customerToUpdate);

        // Iterate over each sale in customerToUpdate
        foreach (var sale in customerToUpdate.Sales)
        {
            // Check if the SaleId is not default (i.e., already exists in the database)
            if (sale.SaleId != default)
            {
                // Mark the sale entity as modified
                CrudDbContext.Entry(sale).State = EntityState.Modified;
            }

            // Check if the SaleId is default or less than 0 (i.e., new sale to be added)
            if (sale.SaleId == default || sale.SaleId < 0)
            {
                // Mark the sale entity as added
                CrudDbContext.Entry(sale).State = EntityState.Added;
            }
        }

        // Iterate over each sale in customerFromDb
        foreach (var saleFromDb in customerFromDb.Sales)
        {
            // Check if any sale in customerToUpdate has the same SaleId as saleFromDb
            if (customerToUpdate.Sales.Any(sale => sale.SaleId == saleFromDb.SaleId))
                continue;

            // Add the saleFromDb to customerToUpdate's sales collection
            customerToUpdate.Sales.Add(saleFromDb);

            // Mark the sale entity as deleted
            CrudDbContext.Entry(customerToUpdate.Sales.Last()).State = EntityState.Deleted;
        }

        // Mark the customerToUpdate entity as modified
        CrudDbContext.Entry(customerToUpdate).State = EntityState.Modified;
    }


    /// <summary>
    /// Gets the CrudDbContext as the underlying CrudDbContext instance.
    /// </summary>
    private CrudDbContext CrudDbContext => Context as CrudDbContext;
}
