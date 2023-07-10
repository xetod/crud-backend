using Crud.Data.DbContexts;
using Crud.Data.Repositories.Customers;
using Crud.Data.Repositories.Products;

namespace Crud.Data.Repositories.Core.UnitOfWorks;

/// <summary>
/// Represents a unit of work implementation that encapsulates a set of related repository operations.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly CrudDbContext _crudDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified CrudDbContext.
    /// </summary>
    /// <param name="crudDbContext">The CrudDbContext used by the unit of work.</param>
    public UnitOfWork(CrudDbContext crudDbContext)
    {
        _crudDbContext = crudDbContext;
        Customer = new CustomerRepository(_crudDbContext);
        Product = new ProductRepository(_crudDbContext);
    }

    /// <summary>
    /// Gets the customer repository.
    /// </summary>
    public ICustomerRepository Customer { get; }

    public IProductRepository Product { get; }

    /// <summary>
    /// Asynchronously saves the changes made in the unit of work to the underlying data store.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation. The task result is the number of objects written to the underlying data store.</returns>
    public async Task<int> CompleteAsync()
    {
        return await _crudDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Disposes the unit of work and the associated CrudDbContext.
    /// </summary>
    public void Dispose()
    {
        _crudDbContext.Dispose();
    }
}
