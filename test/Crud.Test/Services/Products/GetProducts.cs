using AutoMapper;
using Crud.Application.Services.Products.GetProducts;
using Crud.Application.Services.Products.GetProducts.Models;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities.Products;
using Moq;
using System.ComponentModel;

namespace Crud.Test.Services.Products;

/// <summary>
/// Unit tests for the <see cref="GetProducts"/> class.
/// </summary>
[Category("Services")]
public class GetProductsTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProducts _getProducts;

    /// <summary>
    /// Initializes a new instance of the GetProductsTests class.
    /// </summary>
    public GetProductsTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _getProducts = new GetProducts(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    /// <summary>
    /// Tests that ExecuteAsync method returns products when products exist in the database.
    /// </summary>
    [Fact]
    [Trait("ProductServices", "GetProducts")]
    public async Task ExecuteAsync_ShouldReturnProducts_WhenProductsExist()
    {
        // Arrange
        var productsFromDb = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                Name = "Product 1",
                Price = 10
            }
        };
        var mappedProducts = new List<ProductForListDto>
        {
            new ProductForListDto
            {
                ProductId = 1,
                Name = "Product 1",
                Price = 10
            }
        };
        _unitOfWorkMock.Setup(uow => uow.Product.GetProducts()).ReturnsAsync(productsFromDb);
        _mapperMock.Setup(mapper => mapper.Map<List<Product>, List<ProductForListDto>>(productsFromDb))
            .Returns(mappedProducts);

        // Act
        var result = await _getProducts.ExecuteAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(mappedProducts, result.Value);
    }

    /// <summary>
    /// Tests that ExecuteAsync method returns an empty list when no products exist in the database.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        // Arrange
        var productsFromDb = new List<Product>();
        _unitOfWorkMock.Setup(uow => uow.Product.GetProducts()).ReturnsAsync(productsFromDb);

        // Act
        var result = await _getProducts.ExecuteAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Value);
    }

    /// <summary>
    /// Tests that ExecuteAsync method maps products when products exist in the database.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_ShouldMapProducts_WhenProductsExist()
    {
        // Arrange
        var productsFromDb = new List<Product>();
        var productForListDtos = new List<ProductForListDto>();
        _unitOfWorkMock.Setup(uow => uow.Product.GetProducts()).ReturnsAsync(productsFromDb);
        _mapperMock.Setup(mapper => mapper.Map<List<Product>, List<ProductForListDto>>(productsFromDb))
            .Returns(productForListDtos);

        // Act
        var result = await _getProducts.ExecuteAsync();

        // Assert
        _mapperMock.Verify(mapper => mapper.Map<List<Product>, List<ProductForListDto>>(productsFromDb), Times.Once);
        Assert.True(result.Success);
        Assert.Equal(productForListDtos, result.Value);
    }

    /// <summary>
    /// Tests that ExecuteAsync method returns null when the unit of work returns null.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_ShouldReturnProducts_WhenUnitOfWorkReturnsNull()
    {
        // Arrange
        _unitOfWorkMock.Setup(uow => uow.Product.GetProducts()).ReturnsAsync((List<Product>)null);

        // Act
        var result = await _getProducts.ExecuteAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Value);
    }
}
