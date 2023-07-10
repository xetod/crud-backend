using Crud.Data.DbContexts;
using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Repositories.Products;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CrudDbContext context) : base(context) { }

    public async Task<List<Product>> GetProducts()
    {
        return await CrudDbContext
            .Product
            .ToListAsync();
    }

    private CrudDbContext CrudDbContext => Context as CrudDbContext;
}
