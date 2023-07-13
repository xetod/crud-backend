using Crud.Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crud.Data.Configurations;

/// <summary>
/// Entity configuration for the Sale entity.
/// </summary>
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    /// <summary>
    /// Configures the Sale entity.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the Sale entity.</param>
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        // Set the primary key
        builder.HasKey(sale => sale.SaleId);

        // Configure the Date property
        builder.Property(sale => sale.Date)
            .IsRequired();

        // Configure the TotalPrice property with a precision of 8 and scale of 2
        builder.Property(sale => sale.TotalPrice)
            .HasPrecision(8, 2);

        // Configure the relationship with the Customer entity
        builder
            .HasOne(sale => sale.Customer)
            .WithMany(customer => customer.Sales)
            .HasForeignKey(sale => sale.CustomerId);

        // Configure the relationship with the Product entity
        builder
            .HasOne(sale => sale.Product)
            .WithMany(product => product.Sales)
            .HasForeignKey(sale => sale.ProductId);
    }
}
