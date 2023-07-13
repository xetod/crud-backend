using Crud.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crud.Data.Configurations;

/// <summary>
/// Entity configuration for the Customer entity.
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    /// <summary>
    /// Configures the Customer entity.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the Customer entity.</param>
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Set the primary key
        builder.HasKey(customer => customer.CustomerId);

        // Configure the FirstName property
        builder.Property(customer => customer.FirstName)
            .IsRequired();

        // Configure the LastName property
        builder.Property(customer => customer.LastName)
            .IsRequired();

        // Configure the Address property with a maximum length of 200
        builder.Property(customer => customer.Address)
            .HasMaxLength(200);

        // Configure the Email property with a maximum length of 50
        builder.Property(customer => customer.Email)
            .HasMaxLength(50);

        // Configure the PhoneNumber property with a maximum length of 50
        builder.Property(customer => customer.PhoneNumber)
            .HasMaxLength(50);
    }
}
