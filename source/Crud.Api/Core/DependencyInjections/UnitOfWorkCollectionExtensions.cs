using Crud.Data.Repositories.Core.UnitOfWorks;

namespace Crud.Api.Core.DependencyInjections;

/// <summary>
/// Provides extension methods for adding unit of work services to the <see cref="IServiceCollection"/>.
/// </summary>
public static class UnitOfWorkCollectionExtensions
{
    /// <summary>
    /// Adds unit of work services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the unit of work services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddUnitOfWorkServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
