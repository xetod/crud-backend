using Crud.Data.DbContexts;
using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Repositories.Products;

/// <summary>
/// Represents a repository implementation for product entities.
/// </summary>
public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CrudDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves a list of all products.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of products.</returns>
    public async Task<List<Product>> GetProducts()
    {
        return await CrudDbContext
            .Product
            .ToListAsync();
    }

    private CrudDbContext CrudDbContext => Context as CrudDbContext;
}
