using Crud.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Crud.Api.Core.ConfigureDbContexts;

/// <summary>
/// Provides extension methods for configuring the CRUD database context in the application.
/// </summary>
public static class ConfigureDbContext
{
    /// <summary>
    /// Configures the CRUD database context using the specified connection string.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the CRUD database context configuration to.</param>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection ConfigureCrudDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CrudDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }
}
