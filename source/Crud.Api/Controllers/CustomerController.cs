﻿using Crud.Application.Services.Customers.GetCustomer;
using Crud.Application.Services.Customers.GetCustomers;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Api.Controllers
{
    /// <summary>
    /// Controller class for handling customer-related API requests.
    /// </summary>
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IGetCustomers _getCustomers;
        private readonly IGetCustomer _getCustomer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="getCustomers">The service responsible for retrieving customer data.</param>
        public CustomerController(IGetCustomers getCustomers, 
            IGetCustomer getCustomer)
        {
            _getCustomers = getCustomers;
            _getCustomer = getCustomer;
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
            var customers = await _getCustomer.ExecuteAsync(customerId);

            return customers.Success
                ? Ok(customers.Value)
                : StatusCode((int)customers.StatusCode, customers);
        }
    }
}