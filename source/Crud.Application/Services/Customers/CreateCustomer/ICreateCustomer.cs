using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.CreateCustomer.Models;

namespace Crud.Application.Services.Customers.CreateCustomer;

/// <summary>
/// Interface for creating a customer.
/// </summary>
public interface ICreateCustomer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCustomer"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="customerFactory">The customer factory.</param>
    /// <param name="validator">The validator for customer creation data transfer object.</param>
    Task<Result> ExecuteAsync(CustomerForCreateDto model);
}