using AutoMapper;
using Crud.Application.Core.Result;
using Crud.Application.Services.Products.GetProducts.Models;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities;

namespace Crud.Application.Services.Products.GetProducts;

public class GetProducts : IGetProducts
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProducts(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<ProductForListDto>>> ExecuteAsync()
    {
        var productsFromDb = await _unitOfWork.Product.GetProducts();

        var products = _mapper.Map<List<Product>, List<ProductForListDto>>(productsFromDb);

        return Result.Ok(products);
    }
}