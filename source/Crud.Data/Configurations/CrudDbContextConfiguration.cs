using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Configurations;

public static class CrudDbContextConfiguration
{
    public static void ConfigureCrudDbContext(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new SaleConfiguration());
    }
}