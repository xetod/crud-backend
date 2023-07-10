using Crud.Data.Repositories.Customers;
using Crud.Data.Repositories.Products;

namespace Crud.Data.Repositories.Core.UnitOfWorks;

/// <summary>
/// Represents a unit of work interface that encapsulates a set of related repository operations.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the customer repository.
    /// </summary>
    ICustomerRepository Customer { get; }

    IProductRepository Product { get; }

    /// <summary>
    /// Asynchronously saves the changes made in the unit of work to the underlying data store.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation. The task result is the number of objects written to the underlying data store.</returns>
    Task<int> CompleteAsync();
}
