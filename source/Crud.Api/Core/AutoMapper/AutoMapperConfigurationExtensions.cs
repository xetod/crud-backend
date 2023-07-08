using AutoMapper;
using Crud.Application.Core.AutoMapperProfiles;

namespace Crud.Api.Core.AutoMapper;

/// <summary>
/// Provides extension methods for configuring AutoMapper in the application.
/// </summary>
public static class AutoMapperConfigurationExtensions
{
    /// <summary>
    /// Adds AutoMapper configuration to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the AutoMapper configuration to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var mapperConfig = new MapperConfiguration(configurationExpression =>
        {
            // Add mappings defined in CustomerProfile
            configurationExpression.AddProfile(new CustomerProfile());

            // Add mappings defined in SaleProfile
            configurationExpression.AddProfile(new SaleProfile());
        });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
        return services;
    }
}
