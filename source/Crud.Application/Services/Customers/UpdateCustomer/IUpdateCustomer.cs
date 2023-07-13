using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.UpdateCustomer.Models;

namespace Crud.Application.Services.Customers.UpdateCustomer;

/// <summary>
/// Interface for updating a customer.
/// </summary>
public interface IUpdateCustomer
{
    /// <summary>
    /// Executes the customer update operation asynchronously.
    /// </summary>
    /// <param name="model">The customer update data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the update operation.</returns>
    Task<Result> ExecuteAsync(CustomerForUpdateDto model);
}