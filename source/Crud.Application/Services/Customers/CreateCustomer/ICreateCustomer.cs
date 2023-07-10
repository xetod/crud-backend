using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.CreateCustomer.Models;

namespace Crud.Application.Services.Customers.CreateCustomer;

/// <summary>
/// Interface for creating a customer.
/// </summary>
public interface ICreateCustomer
{
    /// <summary>
    /// Executes the operation to create a customer asynchronously.
    /// </summary>
    /// <param name="model">The data transfer object containing the customer information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the operation result.</returns>
    Task<Result> ExecuteAsync(CustomerForCreateDto model);
}