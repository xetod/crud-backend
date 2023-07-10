using Crud.Data.Repositories.Products;
using Crud.Domain.Entities;
using Crud.Test.Helpers;
using System.ComponentModel;

namespace Crud.Test.Repositories.Products;

[Category("ProductRepository")]
public class ProductRepositoryTests
{
    //Get Products
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