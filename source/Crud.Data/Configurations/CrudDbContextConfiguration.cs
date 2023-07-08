namespace Crud.Data.Configurations;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Configuration class for configuring the CRUD DbContext.
/// </summary>
public static class CrudDbContextConfiguration
{
    /// <summary>
    /// Configures the CRUD DbContext by applying entity configurations.
    /// </summary>
    /// <param name="modelBuilder">The model builder used for configuring the DbContext.</param>
    public static void ConfigureCrudDbContext(this ModelBuilder modelBuilder)
    {
        // Apply entity configurations for Customer, Product, and Sale entities
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new SaleConfiguration());
    }
}
