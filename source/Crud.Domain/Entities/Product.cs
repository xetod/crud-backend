namespace Crud.Domain.Entities;

using System.Collections.Generic;

/// <summary>
/// Represents a product entity.
/// </summary>
public class Product
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

    /// <summary>
    /// Gets or sets the sales associated with the product.
    /// </summary>
    public ICollection<Sale> Sales { get; set; }
}
