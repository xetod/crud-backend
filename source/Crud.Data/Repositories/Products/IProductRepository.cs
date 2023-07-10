using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;

namespace Crud.Data.Repositories.Products;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetProducts();
}
