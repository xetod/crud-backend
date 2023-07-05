using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crud.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.ProductId);

        builder.Property(product => product.Name)
            .IsRequired();

        builder.Property(product => product.Price)
            .IsRequired()
            .HasPrecision(8, 2);
    }
}