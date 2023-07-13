using Crud.Application.Core.Result;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities.Customers;

namespace Crud.Application.Services.Customers.DeleteCustomer;

/// <summary>
/// Represents a service for deleting a customer.
/// </summary>
public class DeleteCustomer : IDeleteCustomer
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes a customer with the specified ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the result of the delete operation.
    /// If the operation is successful, the result will be a success result with a null value.
    /// If the customer ID is invalid or the customer is not found, the result will be a failure result with an appropriate error message.
    /// </returns>
    public async Task<Result> ExecuteAsync(int customerId)
    {
        // Check if the customer ID is valid
        if (customerId == default)
            return Result.Fail<Customer>("Customer ID is invalid.");

        // Retrieve the customer from the repository using the customer ID
        var customer = await _unitOfWork.Customer.FirstOrDefaultAsync(cust => cust.CustomerId == customerId);

        // Check if the customer exists
        if (customer == null)
            return Result.Fail<Customer>("Customer not found.");

        // Delete the customer from the repository
        _unitOfWork.Customer.Delete(customer);

        // Complete the unit of work to persist the changes
        var result = await _unitOfWork.CompleteAsync();

        // Return a success result with a null value to indicate successful deletion if the result is greater than 0
        return result > 0
            ? Result.Ok()
            : Result.Fail<Customer>("Deletion failed.");

    }
}