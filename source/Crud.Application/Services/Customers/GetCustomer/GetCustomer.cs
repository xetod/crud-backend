using AutoMapper;
using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.GetCustomer.Models;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities;

namespace Crud.Application.Services.Customers.GetCustomer;

/// <summary>
/// Represents an implementation for retrieving a customer by its ID.
/// </summary>
public class GetCustomer : IGetCustomer
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCustomer(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a customer by its ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer to retrieve.</param>
    /// <returns>
    /// If the customer is found, returns a successful result with the customer data.
    /// If the customer is not found, returns a failed result with an error message.
    /// </returns>
    public async Task<Result<CustomerForDetailDto>> ExecuteAsync(int customerId)
    {
        // Retrieve the customer from the repository using the provided customer ID
        var customer = await _unitOfWork.Customer.GetCustomer(customerId);

        // If the customer is not found, return a failed result with an error message
        if (customer.IsEmpty())
        {
            return Result.Fail<CustomerForDetailDto>("Customer not found.");
        }

        // Map the customer entity to a CustomerForDetailDto using the IMapper
        var result = _mapper.Map<Customer, CustomerForDetailDto>(customer);

        // Return a successful result with the mapped result
        return Result.Ok(result);
    }
}


