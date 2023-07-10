using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.CreateCustomer.Factory;
using Crud.Application.Services.Customers.UpdateCustomer.Models;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Crud.Application.Services.Customers.UpdateCustomer;

/// <summary>
/// Represents a service for updating a customer.
/// </summary>
public class UpdateCustomer : IUpdateCustomer
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerFactory _customerFactory;
    private readonly IValidator<CustomerForUpdateDto> _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCustomer"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="customerFactory">The customer factory.</param>
    /// <param name="validator">The validator for customer update data transfer object.</param>
    public UpdateCustomer(IUnitOfWork unitOfWork, ICustomerFactory customerFactory, IValidator<CustomerForUpdateDto> validator)
    {
        _unitOfWork = unitOfWork;
        _customerFactory = customerFactory;
        _validator = validator;
    }

    /// <summary>
    /// Executes the customer update operation asynchronously.
    /// </summary>
    /// <param name="model">The customer update data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the update operation.</returns>
    public async Task<Result> ExecuteAsync(CustomerForUpdateDto model)
    {
        // Validate the customer update data using the provided validator
        var validationResult = await _validator.ValidateAsync(model);

        if (!validationResult.IsValid)
            // If the validation fails, return a failed result with the validation errors serialized as the error message
            return Result.Fail<Customer>(JsonSerializer.Serialize(validationResult.Errors));

        // Create a new customer entity with the updated data
        var customerToUpdate = _customerFactory.Create(model.CustomerId,
            model.FirstName,
            model.LastName,
            model.Email,
            model.Address,
            model.PhoneNumber,
            model.Sales);

        // Retrieve the customer from the database using the unit of work
        var customerFromDb = await _unitOfWork
            .Customer
            .GetCustomer(model.CustomerId);

        if (customerFromDb == null)
            // If the customer is not found in the database, return a failed result with an appropriate error message and status code
            return Result.Fail<Customer>("Customer not found.", HttpStatusCode.NotFound);

        // Update the customer entity in the unit of work's repository
        _unitOfWork.Customer.Update(customerToUpdate, customerFromDb);

        // Complete the unit of work to persist the changes to the database
        var result = await _unitOfWork.CompleteAsync();

        return result <= 0
            // If the update operation fails, return a failed result with an appropriate error message
            ? Result.Fail<Customer>("Failed to update customer.")
            // If the update operation is successful, return a successful result
            : Result.Ok();
    }
}

