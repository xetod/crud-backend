using Crud.Domain.Entities;

namespace Crud.Test.Entities;

public class SaleTests
{
    [Fact]
    public void QuantityAndUnitPrice_WhenUpdated_TotalPriceRecalculated()
    {
        var sale = new Sale();
        sale.Quantity = 5;
        sale.UnitPrice = 10;

        var totalPrice = sale.TotalPrice;

        Assert.Equal(50, totalPrice);
    }

    [Fact]
    public void SetQuantity_WhenUpdated_TotalPriceRecalculated()
    {
        var sale = new Sale();
        sale.UnitPrice = 10;

        sale.Quantity = 8;
        var totalPrice = sale.TotalPrice;

        Assert.Equal(80, totalPrice);
    }

    [Fact]
    public void SetUnitPrice_WhenUpdated_TotalPriceRecalculated()
    {
        var sale = new Sale();
        sale.Quantity = 5;

        sale.UnitPrice = 15;
        var totalPrice = sale.TotalPrice;

        Assert.Equal(75, totalPrice);
    }
}