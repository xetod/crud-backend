using Crud.Application.Services.Products.GetProducts;

namespace Crud.Api.Core.DependencyInjections.Products;

public static class ProductCollectionExtensions
{
    public static IServiceCollection AddProductServices(this IServiceCollection services)
    {
        services.AddScoped<IGetProducts, GetProducts>();

        return services;
    }
}