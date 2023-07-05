namespace Crud.Domain.Entities;

public class Sale
{
    public int SaleId { get; set; }

    public DateTime Date { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public Customer Customer { get; set; }

    public Product Product { get; set; }

    private int _quantity;
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
    public decimal TotalPrice
    {
        get => _totalPrice;
        private set => _totalPrice = value;
    }
}