using AutoMapper;
using Crud.Application.Core.Result;
using Crud.Application.Services.Products.GetProducts.Models;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities.Products;

namespace Crud.Application.Services.Products.GetProducts;

/// <summary>
/// Implementation of the <see cref="IGetProducts"/> interface for retrieving a list of products.
/// </summary>
public class GetProducts : IGetProducts
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProducts"/> class.
    /// </summary>
    /// <param name="unitOfWork">The <see cref="IUnitOfWork"/> used for data access.</param>
    /// <param name="mapper">The <see cref="IMapper"/> used for object mapping.</param>
    public GetProducts(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a list of products asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
    public async Task<Result<List<ProductForListDto>>> ExecuteAsync()
    {
        var productsFromDb = await _unitOfWork.Product.GetProducts();

        var products = _mapper.Map<List<Product>, List<ProductForListDto>>(productsFromDb);

        return Result.Ok(products);
    }
}
