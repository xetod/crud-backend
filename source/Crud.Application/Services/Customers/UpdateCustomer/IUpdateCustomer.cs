using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.UpdateCustomer.Models;

namespace Crud.Application.Services.Customers.UpdateCustomer;

/// <summary>
/// Interface for updating a customer.
/// </summary>
public interface IUpdateCustomer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCustomer"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="customerFactory">The customer factory.</param>
    /// <param name="validator">The validator for customer update data transfer object.</param>
    Task<Result> ExecuteAsync(CustomerForUpdateDto model);
}