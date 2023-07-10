namespace Crud.Application.Services.Products.GetProducts.Models;

/// <summary>
/// Data transfer object (DTO) for representing a product in a list.
/// </summary>
public class ProductForListDto
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }
}