using Crud.Data.Repositories.Products;
using Crud.Domain.Entities.Products;
using Crud.Test.Helpers;
using System.ComponentModel;

namespace Crud.Test.Repositories.Products;

/// <summary>
/// Integration tests for the <see cref="ProductRepository"/> class.
/// </summary>
[Category("Repositories")]
public class ProductRepositoryTests
{
    /// <summary>
    /// Unit test for the <see cref="ProductRepository.GetProducts"/> method.
    /// It verifies that all products are returned when products exist in the database.
    /// </summary>
    [Fact]
    [Trait("ProductRepository", "GetProducts")]
    public async Task GetProducts_ReturnsAllProducts()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        var expectedProducts = new List<Product>
        {
            new Product { Name = "Product 1" },
            new Product { Name = "Product 2" },
            new Product { Name = "Product 3" }
        };

        dbContext.Product.AddRange(expectedProducts);
        await dbContext.SaveChangesAsync();

        var sut = new ProductRepository(dbContext);

        // Act
        var actualProducts = await sut.GetProducts();

        // Assert
        Assert.Equal(expectedProducts.Count, actualProducts.Count);
        Assert.Equal(expectedProducts.Select(product => product.ProductId), actualProducts.Select(p => p.ProductId));
        Assert.Equal(expectedProducts.Select(product => product.Name), actualProducts.Select(p => p.Name));
    }

    /// <summary>
    /// Unit test for the <see cref="ProductRepository.GetProducts"/> method.
    /// It verifies that an empty list is returned when no products exist in the database.
    /// </summary>
    [Fact]
    [Trait("ProductRepository", "GetProducts")]
    public async Task GetProducts_ReturnsEmptyList_WhenNoProductsExist()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();
        var sut = new ProductRepository(dbContext);

        // Act
        var actualProducts = await sut.GetProducts();

        // Assert
        Assert.Empty(actualProducts);
    }

    /// <summary>
    /// Unit test for the <see cref="ProductRepository.GetProducts"/> method.
    /// It verifies that the correct products are returned after adding a new product to the database.
    /// </summary>
    [Fact]
    [Trait("ProductRepository", "GetProducts")]
    public async Task GetProducts_ReturnsCorrectProducts_AfterAddingNewProduct()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();
        var sut = new ProductRepository(dbContext);

        var newProduct = new Product { Name = "Product 4" };
        dbContext.Product.Add(newProduct);
        await dbContext.SaveChangesAsync();

        // Act
        var actualProducts = await sut.GetProducts();

        // Assert
        Assert.Contains(newProduct, actualProducts);
    }
}
