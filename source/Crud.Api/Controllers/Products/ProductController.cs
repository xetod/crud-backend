using Crud.Application.Services.Products.GetProducts;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Api.Controllers.Products;

/// <summary>
/// Controller for handling product-related operations.
/// </summary>
[Route("api")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IGetProducts _getProducts;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductController"/> class.
    /// </summary>
    /// <param name="getProducts">The service for retrieving products.</param>
    public ProductController(IGetProducts getProducts)
    {
        _getProducts = getProducts;
    }

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>The HTTP action result containing the products if successful; otherwise, an error status code.</returns>
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _getProducts.ExecuteAsync();

        return products.Success
            ? Ok(products.Value)
            : StatusCode((int)products.StatusCode, products);
    }
}

