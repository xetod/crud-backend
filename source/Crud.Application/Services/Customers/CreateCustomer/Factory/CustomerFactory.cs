using Crud.Application.Services.Customers.CreateCustomer.Models;
using Crud.Application.Services.Customers.UpdateCustomer.Models;
using Crud.Domain.Entities.Customers;
using Crud.Domain.Entities.Sales;

namespace Crud.Application.Services.Customers.CreateCustomer.Factory;

/// <summary>
/// Represents a concrete implementation of the customer factory.
/// </summary>
public class CustomerFactory : ICustomerFactory
{
    /// <summary>
    /// Creates a new customer entity with the specified details and maps the sale DTOs to sale entities.
    /// </summary>
    /// <param name="firstName">The first name of the customer.</param>
    /// <param name="lastName">The last name of the customer.</param>
    /// <param name="email">The email address of the customer.</param>
    /// <param name="address">The address of the customer.</param>
    /// <param name="phoneNumber">The phone number of the customer.</param>
    /// <param name="sales">The list of sale DTOs.</param>
    /// <returns>The created customer entity.</returns>
    public Customer Create(string firstName, string lastName, string email, string address, string phoneNumber, List<SaleForCreateDto> sales)
    {
        // Create a new customer entity with the specified details
        return new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Address = address,
            PhoneNumber = phoneNumber,
            Sales = sales != null
                ? sales.Select(dto => new Sale
                {
                    Date = DateTime.Now,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice
                }).ToList()
                : new List<Sale>()
        };
    }

    /// <summary>
    /// Creates a new customer entity with the specified details and maps the sale DTOs to sale entities.
    /// </summary>
    /// <param name="customerId">The ID of the customer</param>
    /// <param name="firstName">The first name of the customer.</param>
    /// <param name="lastName">The last name of the customer.</param>
    /// <param name="email">The email address of the customer.</param>
    /// <param name="address">The address of the customer.</param>
    /// <param name="phoneNumber">The phone number of the customer.</param>
    /// <param name="sales">The list of sale DTOs.</param>
    /// <returns>The created customer entity.</returns>
    public Customer Create(int customerId, string firstName, string lastName, string email, string address, string phoneNumber, List<SaleForUpdateDto> sales)
    {
        // Create a new customer entity with the specified details
        return new Customer
        {
            CustomerId = customerId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Address = address,
            PhoneNumber = phoneNumber,
            Sales = sales != null
                ? sales.Select(dto => new Sale()
                {
                    SaleId = dto.SaleId,
                    Date = DateTime.Now,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice
                }).ToList()
                : new List<Sale>()
        };
    }
}