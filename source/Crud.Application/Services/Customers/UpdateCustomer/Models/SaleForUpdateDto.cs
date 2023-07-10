namespace Crud.Application.Services.Customers.UpdateCustomer.Models;

/// <summary>
/// Represents the data transfer object (DTO) for creating a new sale.
/// </summary>
public class SaleForUpdateDto
{
    /// <summary>
    /// Gets or sets the sale ID.
    /// </summary>
    public int SaleId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the product associated with the sale.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product associated with the sale.
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}