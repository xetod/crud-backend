using Crud.Application.Services.Customers.GetCustomer;
using Crud.Application.Services.Customers.GetCustomers;

namespace Crud.Api.Core.DependencyInjections;

/// <summary>
/// Provides extension methods for adding customer-related services to the <see cref="IServiceCollection"/>.
/// </summary>
public static class CustomerCollectionExtensions
{
    /// <summary>
    /// Adds customer-related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the customer services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddCustomerServices(this IServiceCollection services)
    {
        services.AddScoped<IGetCustomers, GetCustomers>();
        services.AddScoped<IGetCustomer, GetCustomer>();

        return services;
    }
}