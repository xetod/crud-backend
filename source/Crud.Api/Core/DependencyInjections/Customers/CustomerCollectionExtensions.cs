using Crud.Application.Services.Customers.CreateCustomer;
using Crud.Application.Services.Customers.CreateCustomer.Factory;
using Crud.Application.Services.Customers.CreateCustomer.Models;
using Crud.Application.Services.Customers.CreateCustomer.Validation;
using Crud.Application.Services.Customers.GetCustomer;
using Crud.Application.Services.Customers.GetCustomers;
using Crud.Application.Services.Customers.UpdateCustomer;
using Crud.Application.Services.Customers.UpdateCustomer.Models;
using Crud.Application.Services.Customers.UpdateCustomer.Validation;
using FluentValidation;

namespace Crud.Api.Core.DependencyInjections.Customers;

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
        services.AddScoped<ICreateCustomer, CreateCustomer>();
        services.AddScoped<IUpdateCustomer, UpdateCustomer>();
        services.AddScoped<ICustomerFactory, CustomerFactory>();
        services.AddScoped<IValidator<CustomerForCreateDto>, CustomerForCreationValidator>();
        services.AddScoped<IValidator<CustomerForUpdateDto>, CustomerForUpdateValidator>();

        return services;
    }
}