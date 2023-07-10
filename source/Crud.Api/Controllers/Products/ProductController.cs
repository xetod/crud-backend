using Crud.Application.Services.Products.GetProducts;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Api.Controllers.Products;

[Route("api")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IGetProducts _getProducts;

    public ProductController(IGetProducts getProducts)
    {
        _getProducts = getProducts;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _getProducts.ExecuteAsync();

        return products.Success
            ? Ok(products.Value)
            : StatusCode((int)products.StatusCode, products);
    }
}

