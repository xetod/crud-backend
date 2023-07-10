using Crud.Application.Core.Result;
using Crud.Application.Services.Products.GetProducts.Models;

namespace Crud.Application.Services.Products.GetProducts;

public interface IGetProducts
{
    Task<Result<List<ProductForListDto>>> ExecuteAsync();
}