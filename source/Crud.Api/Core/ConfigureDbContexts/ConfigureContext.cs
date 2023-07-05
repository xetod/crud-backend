using Crud.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Crud.Api.Core.ConfigureDbContexts;

public static class ConfigureDbContext
{
    public static IServiceCollection ConfigureCrudDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CrudDbContext>(options => options.UseSqlServer(connectionString));
        
        return services;
    }
}