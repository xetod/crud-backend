using Crud.Domain.Entities;

namespace Crud.Test.Entities;

/// <summary>
/// Unit tests for the Sale class.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that when the Quantity and UnitPrice properties of a Sale instance are updated, the TotalPrice property is recalculated correctly.
    /// </summary>
    [Fact]
    public void QuantityAndUnitPrice_WhenUpdated_TotalPriceRecalculated()
    {
        var sale = new Sale();
        sale.Quantity = 5;
        sale.UnitPrice = 10;

        var totalPrice = sale.TotalPrice;

        Assert.Equal(50, totalPrice);
    }

    /// <summary>
    /// Tests that when the Quantity property of a Sale instance is updated, the TotalPrice property is recalculated correctly.
    /// </summary>
    [Fact]
    public void SetQuantity_WhenUpdated_TotalPriceRecalculated()
    {
        var sale = new Sale();
        sale.UnitPrice = 10;

        sale.Quantity = 8;
        var totalPrice = sale.TotalPrice;

        Assert.Equal(80, totalPrice);
    }

    /// <summary>
    /// Tests that when the UnitPrice property of a Sale instance is updated, the TotalPrice property is recalculated correctly.
    /// </summary>
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
