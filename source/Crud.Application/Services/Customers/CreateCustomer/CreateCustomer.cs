using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.CreateCustomer.Factory;
using Crud.Application.Services.Customers.CreateCustomer.Models;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities;
using FluentValidation;
using System.Text.Json;

namespace Crud.Application.Services.Customers.CreateCustomer;

/// <summary>
/// Represents a service for creating a customer.
/// </summary>
public class CreateCustomer : ICreateCustomer
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerFactory _customerFactory;
    private readonly IValidator<CustomerForCreateDto> _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCustomer"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="customerFactory">The customer factory.</param>
    /// <param name="validator">The validator for customer creation data transfer object.</param>
    public CreateCustomer(IUnitOfWork unitOfWork, ICustomerFactory customerFactory, IValidator<CustomerForCreateDto> validator)
    {
        _unitOfWork = unitOfWork;
        _customerFactory = customerFactory;
        _validator = validator;
    }

    /// <summary>
    /// Executes the customer creation operation asynchronously.
    /// </summary>
    /// <param name="model">The customer creation data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the creation operation.</returns>
    public async Task<Result> ExecuteAsync(CustomerForCreateDto model)
    {
        // Validate the customer creation data using the provided validator
        var validationResult = await _validator.ValidateAsync(model);

        if (!validationResult.IsValid)
            // If the validation fails, return a failed result with the validation errors serialized as the error message
            return Result.Fail<Customer>(JsonSerializer.Serialize(validationResult.Errors));

        // Create a new customer entity with the provided data
        var customer = _customerFactory.Create(model.FirstName,
            model.LastName,
            model.Email,
            model.Address,
            model.PhoneNumber,
            model.Sales);

        // Add the customer entity to the unit of work's repository
        await _unitOfWork.Customer.AddAsync(customer);

        // Complete the unit of work to persist the changes to the database
        var result = await _unitOfWork.CompleteAsync();

        return result <= 0
            // If the creation operation fails, return a failed result with an appropriate error message
            ? Result.Fail<Customer>("Failed to save customer.")
            // If the creation operation is successful, return a successful result
            : Result.Ok();
    }
}
