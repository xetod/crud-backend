using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;

namespace Crud.Data.Repositories.Products;

/// <summary>
/// Represents a repository interface for product entities.
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    /// <summary>
    /// Retrieves a list of all products.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of products.</returns>
    Task<List<Product>> GetProducts();
}
