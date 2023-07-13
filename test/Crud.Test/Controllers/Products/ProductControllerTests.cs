using Crud.Api.Controllers.Products;
using Crud.Application.Core.Result;
using Crud.Application.Services.Products.GetProducts;
using Crud.Application.Services.Products.GetProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel;
using System.Net;

namespace Crud.Test.Controllers.Products;

/// <summary>
/// Represents unit tests for the <see cref="ProductController"/> class.
/// </summary>
[Category("ProductController")]
public class ProductControllerTests
{
    /// <summary>
    /// Tests the <see cref="ProductController.GetProducts"/> method and verifies that it returns an <see cref="OkObjectResult"/> when products are retrieved successfully.
    /// </summary>
    [Fact]
    [Trait("ProductController", "GetProducts")]
    public async Task GetProducts_ReturnsOkResult_WhenProductsSuccessful()
    {
        // Arrange
        var mockGetProducts = new Mock<IGetProducts>();
        var products = new List<ProductForListDto>();
        var expectedResult = Result.Ok<List<ProductForListDto>>(products);

        mockGetProducts.Setup(getProducts => getProducts.ExecuteAsync()).ReturnsAsync(expectedResult);

        var controller = new ProductController(mockGetProducts.Object);

        // Act
        var result = await controller.GetProducts();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.IsType<List<ProductForListDto>>(okResult.Value);
    }

    /// <summary>
    /// Tests the <see cref="ProductController.GetProducts"/> method and verifies that it returns the appropriate status code when an error occurs.
    /// </summary>
    [Fact]
    [Trait("ProductController", "GetProducts")]
    public async Task GetProducts_Returns_StatusCode_When_Failure()
    {
        // Arrange
        var mockGetProducts = new Mock<IGetProducts>();
        var failureMessage = "Some error message";
        var statusCode = HttpStatusCode.BadRequest;
        var failureResult = Result.Fail<List<ProductForListDto>>(failureMessage, statusCode);

        mockGetProducts.Setup(getProducts => getProducts.ExecuteAsync()).ReturnsAsync(failureResult);

        var controller = new ProductController(mockGetProducts.Object);

        // Act
        var result = await controller.GetProducts();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal((int)statusCode, statusCodeResult.StatusCode);
    }
}