using Crud.Application.Services.Products.GetProducts;

namespace Crud.Api.Core.DependencyInjections.Products;

/// <summary>
/// Extension methods for adding product-related services to the <see cref="IServiceCollection"/>.
/// </summary>
public static class ProductCollectionExtensions
{
    /// <summary>
    /// Adds product services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddProductServices(this IServiceCollection services)
    {
        services.AddScoped<IGetProducts, GetProducts>();

        return services;
    }
}