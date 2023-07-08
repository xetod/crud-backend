using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crud.Data.Configurations;

/// <summary>
/// Entity configuration for the Product entity.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the Product entity.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the Product entity.</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Set the primary key
        builder.HasKey(product => product.ProductId);

        // Configure the Name property
        builder.Property(product => product.Name)
            .IsRequired();

        // Configure the Price property
        builder.Property(product => product.Price)
            .IsRequired()
            .HasPrecision(8, 2);
    }
}
