namespace Crud.Domain.Entities;

/// <summary>
/// Represents a sale entity.
/// </summary>
public class Sale
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
    /// Gets or sets the customer associated with the sale.
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    /// Gets or sets the product associated with the sale.
    /// </summary>
    public Product Product { get; set; }

    private int _quantity;
    /// <summary>
    /// Gets or sets the quantity of the product sold.
    /// </summary>
    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            _totalPrice = _unitPrice * _quantity;
        }
    }

    private decimal _unitPrice;
    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice
    {
        get => _unitPrice;
        set
        {
            _unitPrice = value;
            _totalPrice = _unitPrice * _quantity;
        }
    }

    private decimal _totalPrice;
    /// <summary>
    /// Gets the total price of the sale (calculated as unit price multiplied by quantity).
    /// </summary>
    public decimal TotalPrice
    {
        get => _totalPrice;
        private set => _totalPrice = value;
    }
}