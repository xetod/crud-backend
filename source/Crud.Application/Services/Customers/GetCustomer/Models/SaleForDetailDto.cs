namespace Crud.Application.Services.Customers.GetCustomer.Models;

/// <summary>
/// Represents the data transfer object (DTO) for displaying sale details.
/// </summary>
public class SaleForDetailDto
{
    /// <summary>
    /// Gets or sets the sale ID.
    /// </summary>
    public int SaleId { get; set; }

    /// <summary>
    /// Gets or sets the date of the sale.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the customer ID associated with the sale.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the product ID associated with the sale.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets the total price of the sale (calculated as unit price multiplied by quantity).
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Gets or sets the name of the product associated with the sale.
    /// </summary>
    public string ProductName { get; set; }
}