using Crud.Application.Core.Result;
using Crud.Application.Services.Products.GetProducts.Models;

namespace Crud.Application.Services.Products.GetProducts;

/// <summary>
/// Interface for retrieving a list of products.
/// </summary>
public interface IGetProducts
{
    /// <summary>
    /// Retrieves a list of products asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
    Task<Result<List<ProductForListDto>>> ExecuteAsync();
}