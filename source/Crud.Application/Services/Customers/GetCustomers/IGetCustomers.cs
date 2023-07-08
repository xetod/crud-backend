using Crud.Application.Core.ResourceParameters;
using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.GetCustomers.Models;

namespace Crud.Application.Services.Customers.GetCustomers;

/// <summary>
/// Interface for retrieving customers with pagination.
/// </summary>
public interface IGetCustomers
{
    /// <summary>
    /// Retrieves a collection of customers with pagination based on the provided parameters.
    /// </summary>
    /// <param name="parameter">The customer resource parameter containing query parameters.</param>
    /// <returns>A task representing the asynchronous operation with a result containing the collection of customers with pagination metadata.</returns>
    Task<Result<CollectionResource<CustomerForListDto>>> ExecuteAsync(CustomerResourceParameter parameter);
}
