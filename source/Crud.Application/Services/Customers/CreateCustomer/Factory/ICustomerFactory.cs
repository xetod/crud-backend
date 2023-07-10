using Crud.Application.Services.Customers.CreateCustomer.Models;
using Crud.Application.Services.Customers.UpdateCustomer.Models;
using Crud.Domain.Entities;

namespace Crud.Application.Services.Customers.CreateCustomer.Factory;

/// <summary>
/// Represents a factory for creating customer entities.
/// </summary>
public interface ICustomerFactory
{
    /// <summary>
    /// Creates a new customer entity with the specified details.
    /// </summary>
    /// <param name="firstName">The first name of the customer.</param>
    /// <param name="lastName">The last name of the customer.</param>
    /// <param name="email">The email address of the customer.</param>
    /// <param name="address">The address of the customer.</param>
    /// <param name="phoneNumber">The phone number of the customer.</param>
    /// <param name="sales">The sales associated with the customer.</param>
    /// <returns>The newly created customer entity.</returns>
    Customer Create(string firstName, string lastName, string email, string address, string phoneNumber, List<SaleForCreateDto> sales);

    /// <summary>
    /// Creates a new customer entity with the specified details.
    /// </summary>
    /// <param name="customerId">The ID of the customer</param>
    /// <param name="firstName">The first name of the customer.</param>
    /// <param name="lastName">The last name of the customer.</param>
    /// <param name="email">The email address of the customer.</param>
    /// <param name="address">The address of the customer.</param>
    /// <param name="phoneNumber">The phone number of the customer.</param>
    /// <param name="sales">The sales associated with the customer.</param>
    /// <returns>The newly created customer entity.</returns>
    Customer Create(int customerId, string firstName, string lastName, string email, string address, string phoneNumber, List<SaleForUpdateDto> sales);
}