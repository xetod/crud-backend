using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crud.Data.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(sale => sale.SaleId);

        builder.Property(sale => sale.Date)
            .IsRequired();

        builder.Property(sale => sale.TotalPrice)
            .HasPrecision(8, 2);

        builder
            .HasOne(sale => sale.Customer)
            .WithMany(customer => customer.Sales)
            .HasForeignKey(sale => sale.CustomerId);

        builder
            .HasOne(sale => sale.Product)
            .WithMany(product => product.Sales)
            .HasForeignKey(sale => sale.ProductId);
    }
}