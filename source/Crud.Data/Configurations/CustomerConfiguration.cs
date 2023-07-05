using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crud.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(customer => customer.CustomerId);

        builder.Property(customer => customer.Name)
            .IsRequired();

        builder.Property(customer => customer.Address)
            .HasMaxLength(200);

        builder.Property(customer => customer.Email)
            .HasMaxLength(50);

        builder.Property(customer => customer.PhoneNumber)
            .HasMaxLength(50);
    }
}