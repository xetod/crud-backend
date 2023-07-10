using Crud.Application.Services.Customers.CreateCustomer;
using Crud.Application.Services.Customers.CreateCustomer.Models;
using Crud.Application.Services.Customers.DeleteCustomer;
using Crud.Application.Services.Customers.GetCustomer;
using Crud.Application.Services.Customers.GetCustomers;
using Crud.Application.Services.Customers.UpdateCustomer;
using Crud.Application.Services.Customers.UpdateCustomer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Api.Controllers.Customers;

/// <summary>
/// Controller class for handling customer-related API requests.
/// </summary>
[Route("api")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IGetCustomers _getCustomers;
    private readonly IGetCustomer _getCustomer;
    private readonly ICreateCustomer _createCustomer;
    private readonly IUpdateCustomer _updateCustomer;
    private readonly IDeleteCustomer _deleteCustomer;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerController"/> class.
    /// </summary>
    /// <param name="getCustomers">The service responsible for retrieving customers data.</param>
    /// <param name="getCustomer">The service responsible for retrieving a single customer.</param>
    /// <param name="createCustomer">The service responsible for creating a new customer.</param>
    /// <param name="updateCustomer">The service responsible for updating an existing customer.</param>
    public CustomerController(IGetCustomers getCustomers,
        IGetCustomer getCustomer,
        ICreateCustomer createCustomer,
        IUpdateCustomer updateCustomer,
        IDeleteCustomer deleteCustomer)
    {
        _getCustomers = getCustomers;
        _getCustomer = getCustomer;
        _createCustomer = createCustomer;
        _updateCustomer = updateCustomer;
        _deleteCustomer = deleteCustomer;
    }


    /// <summary>
    /// Retrieves a list of customers based on the provided parameters.
    /// </summary>
    /// <param name="parameter">The query parameter object containing filter and pagination parameters.</param>
    /// <returns>
    /// Returns an HTTP 200 OK response with the list of customers as the response body if the operation is successful.
    /// Otherwise, returns an HTTP status code and error details.
    /// </returns>
    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers([FromQuery] CustomerResourceParameter parameter)
    {
        var customers = await _getCustomers.ExecuteAsync(parameter);

        return customers.Success
            ? Ok(customers.Value)
            : StatusCode((int)customers.StatusCode, customers);
    }

    /// <summary>
    /// Retrieves a customer by its ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer to retrieve.</param>
    /// <returns>
    /// If the operation is successful, returns Ok result with the customer data.
    /// If the operation fails, returns the appropriate error status code and error details.
    /// </returns>
    [HttpGet("customers/{customerId}")]
    public async Task<IActionResult> GetCustomer(int customerId)
    {
        var customer = await _getCustomer.ExecuteAsync(customerId);

        return customer.Success
            ? Ok(customer.Value)
            : StatusCode((int)customer.StatusCode, customer);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="model">The data for creating the customer.</param>
    /// <returns>
    /// If the operation is successful, returns Ok result.
    /// If the operation fails, returns the appropriate error status code and error details.
    /// </returns>
    [HttpPost("customer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerForCreateDto model)
    {
        var customer = await _createCustomer.ExecuteAsync(model);

        return customer.Success
            ? Ok()
            : StatusCode((int)customer.StatusCode, customer);
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="model">The data for updating the customer.</param>
    /// <returns>
    /// If the operation is successful, returns Ok result.
    /// If the operation fails, returns the appropriate error status code and error details.
    /// </returns>
    [HttpPut("customers/{customerId}")]
    public async Task<IActionResult> UpdateCustomer([FromBody] CustomerForUpdateDto model)
    {
        var customer = await _updateCustomer.ExecuteAsync(model);

        return customer.Success
            ? Ok()
            : StatusCode((int)customer.StatusCode, customer);
    }

    /// <summary>
    /// Deletes a customer with the specified ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer to delete.</param>
    /// <returns>
    /// If the operation is successful, returns an HTTP 204 No Content response.
    /// If the operation fails, returns the appropriate error status code and error details.
    /// </returns>
    [HttpDelete("customers/{customerId}")]
    public async Task<IActionResult> DeleteCustomer(int customerId)
    {
        var result = await _deleteCustomer.ExecuteAsync(customerId);

        return result.Success
            ? NoContent()
            : StatusCode((int)result.StatusCode, result);
    }

}