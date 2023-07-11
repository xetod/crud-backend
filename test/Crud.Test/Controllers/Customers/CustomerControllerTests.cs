using System.ComponentModel;
using System.Net;
using AutoMapper;
using Crud.Api.Controllers.Customers;
using Crud.Application.Core.AutoMapperProfiles.Customers;
using Crud.Application.Core.AutoMapperProfiles.Sales;
using Crud.Application.Core.ResourceParameters;
using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.CreateCustomer;
using Crud.Application.Services.Customers.CreateCustomer.Models;
using Crud.Application.Services.Customers.DeleteCustomer;
using Crud.Application.Services.Customers.GetCustomer;
using Crud.Application.Services.Customers.GetCustomer.Models;
using Crud.Application.Services.Customers.GetCustomers;
using Crud.Application.Services.Customers.GetCustomers.Models;
using Crud.Application.Services.Customers.UpdateCustomer;
using Crud.Application.Services.Customers.UpdateCustomer.Models;
using Crud.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Crud.Test.Controllers.Customers;

[Category("Controllers")]
public class CustomerControllerTests
{
    private readonly CustomerController _customerController;
    private readonly Mock<IGetCustomers> _mockGetCustomers;
    private readonly Mock<IGetCustomer> _mockGetCustomer;
    private readonly Mock<ICreateCustomer> _mockCreateCustomer;
    private readonly Mock<IUpdateCustomer> _mockUpdateCustomer;
    private readonly Mock<IDeleteCustomer> _mockDeleteCustomer;
    private readonly IMapper _mapper;

    public CustomerControllerTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CustomerProfile>();
            cfg.AddProfile<SaleProfile>();
        });
        _mapper = new Mapper(mapperConfiguration);
        _mockGetCustomers = new Mock<IGetCustomers>();
        _mockGetCustomer = new Mock<IGetCustomer>();
        _mockCreateCustomer = new Mock<ICreateCustomer>();
        _mockUpdateCustomer = new Mock<IUpdateCustomer>();
        _mockDeleteCustomer = new Mock<IDeleteCustomer>();

        _customerController = new CustomerController(
            _mockGetCustomers.Object,
            _mockGetCustomer.Object,
            _mockCreateCustomer.Object,
            _mockUpdateCustomer.Object,
            _mockDeleteCustomer.Object
        );
    }

    // Get Customers
    /// <summary>
    /// Unit test for the <see cref="CustomerController.GetCustomers"/> action.
    /// It verifies that the action returns an <see cref="OkObjectResult"/> with a collection of customers when a valid parameter is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerController", "GetCustomers")]
    public async Task GetCustomers_WithValidParameter_ReturnsOkResultWithCustomers()
    {
        // Arrange
        var parameter = new CustomerResourceParameter();

        var customers = GetCustomerSamples();

        var paginationMetadata = new PaginationMetadata
        {
            TotalCount = 5,
            PageSize = 2,
            CurrentPage = 1,
            TotalPages = 3
        };

        var collectionResource = new CollectionResource<CustomerForListDto>
        {
            Results = _mapper.Map<List<Customer>, List<CustomerForListDto>>(customers),
            Pagination = paginationMetadata
        };

        var expectedResult = Result.Ok(collectionResource);

        _mockGetCustomers
            .Setup(x => x.ExecuteAsync(parameter))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _customerController.GetCustomers(parameter) as OkObjectResult;
        var list = result.Value as CollectionResource<CustomerForListDto>;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(list);
        Assert.Equal(5, list.Results.Count);
        Assert.Equal("Jane Smith", $"{list.Results[0].FirstName} {list.Results[0].LastName}");
        Assert.Equal("Phil Boyce", $"{list.Results[1].FirstName} {list.Results[1].LastName}");
    }




    // Get Customer
    /// <summary>
    /// Unit test for the <see cref="CustomerController.GetCustomer"/> action.
    /// It verifies that the action returns an <see cref="OkObjectResult"/> with customer data when a valid customer ID is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerController", "GetCustomer")]
    public async Task GetCustomer_CustomerFound_ReturnsOkResultWithData()
    {
        // Arrange
        var customerId = 123;
        var customerData = new CustomerForDetailDto { CustomerId = customerId, FirstName = "John" };
        var successfulResult = Result.Ok<CustomerForDetailDto>(customerData);

        _mockGetCustomer
            .Setup(mock => mock.ExecuteAsync(customerId))
            .ReturnsAsync(successfulResult);

        // Act
        var result = await _customerController.GetCustomer(customerId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(customerData, okResult.Value);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerController.GetCustomer"/> action.
    /// It verifies that the action returns an <see cref="ObjectResult"/> with the appropriate status code and result object when a customer is not found.
    /// </summary>
    [Fact]
    [Trait("CustomerController", "GetCustomer")]
    public async Task GetCustomer_CustomerNotFound_ReturnsStatusCodeWithResultObject()
    {
        // Arrange
        var customerId = 123;
        var errorMessage = "Customer not found.";
        var statusCode = HttpStatusCode.NotFound;
        var failedResult = Result.Fail<CustomerForDetailDto>(errorMessage);

        _mockGetCustomer
            .Setup(mock => mock.ExecuteAsync(customerId))
            .ReturnsAsync(failedResult);

        // Act
        var result = await _customerController.GetCustomer(customerId);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal((int)failedResult.StatusCode, objectResult.StatusCode);
        Assert.Equal(failedResult, objectResult.Value);
    }




    // Create Customer
    /// <summary>
    /// Unit test for the <see cref="CustomerController.CreateCustomer"/> action.
    /// It verifies that the action returns an <see cref="OkResult"/> when a valid customer model is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerController", "CreateCustomer")]
    public async Task CreateCustomer_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var model = new CustomerForCreateDto();
        var successResult = Result.Ok();

        _mockCreateCustomer
            .Setup(createCustomer => createCustomer.ExecuteAsync(model))
            .ReturnsAsync(successResult);

        // Act
        var result = await _customerController.CreateCustomer(model);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerController.CreateCustomer"/> action.
    /// It verifies that the action returns an <see cref="ObjectResult"/> with the appropriate status code when an invalid customer model is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerController", "CreateCustomer")]
    public async Task CreateCustomer_WithInvalidModel_ReturnsErrorResult()
    {
        // Arrange
        var model = new CustomerForCreateDto();
        var errorMessage = "Failed to save customer.";
        var statusCode = HttpStatusCode.BadRequest;
        var failedResult = Result.Fail<CustomerForCreateDto>(errorMessage);

        _mockCreateCustomer
            .Setup(createCustomer => createCustomer.ExecuteAsync(model))
            .ReturnsAsync(failedResult);

        // Act
        var result = await _customerController.CreateCustomer(model);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal((int)statusCode, objectResult.StatusCode);
    }




    // Update Customer
    /// <summary>
    /// Unit test for the <see cref="CustomerController.UpdateCustomer"/> action.
    /// It verifies that the action returns an <see cref="OkResult"/> when a valid customer model is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerController", "UpdateCustomer")]
    public async Task UpdateCustomer_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var model = new CustomerForUpdateDto();
        var successResult = Result.Ok();

        _mockUpdateCustomer
            .Setup(updateCustomer => updateCustomer.ExecuteAsync(model))
            .ReturnsAsync(successResult);

        // Act
        var result = await _customerController.UpdateCustomer(model);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerController.UpdateCustomer"/> action.
    /// It verifies that the action returns an <see cref="ObjectResult"/> with the appropriate status code when an invalid customer model is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerController", "UpdateCustomer")]
    public async Task UpdateCustomer_WithInvalidModel_ReturnsErrorResult()
    {
        // Arrange
        var model = new CustomerForUpdateDto();
        var errorMessage = "Failed to save customer.";
        var statusCode = HttpStatusCode.BadRequest;
        var failedResult = Result.Fail<CustomerForUpdateDto>(errorMessage);

        _mockUpdateCustomer
            .Setup(updateCustomer => updateCustomer.ExecuteAsync(model))
            .ReturnsAsync(failedResult);

        // Act
        var result = await _customerController.UpdateCustomer(model);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal((int)statusCode, objectResult.StatusCode);
    }




    //Delete Customer
    /// <summary>
    /// Unit test for the <see cref="CustomerController.DeleteCustomer"/> action.
    /// It verifies that the action returns a <see cref="NoContentResult"/> when a valid customer ID is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerController", "DeleteCustomer")]
    public async Task DeleteCustomer_WithValidCustomerId_ReturnsNoContentResult()
    {
        // Arrange
        int customerId = 1;
        var successResult = Result.Ok();

        _mockDeleteCustomer
            .Setup(deleteCustomer => deleteCustomer.ExecuteAsync(customerId))
            .ReturnsAsync(successResult);

        // Act
        var result = await _customerController.DeleteCustomer(customerId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerController.DeleteCustomer"/> action.
    /// It verifies that the action returns an <see cref="ObjectResult"/> with the appropriate status code when an invalid customer ID is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerController", "DeleteCustomer")]
    public async Task DeleteCustomer_WithInvalidCustomerId_ReturnsErrorResult()
    {
        // Arrange
        int customerId = 1;
        var errorMessage = "Failed to save customer.";
        var statusCode = HttpStatusCode.BadRequest;
        var failedResult = Result.Fail<CustomerForUpdateDto>(errorMessage);

        _mockDeleteCustomer
            .Setup(deleteCustomer => deleteCustomer.ExecuteAsync(customerId))
            .ReturnsAsync(failedResult);

        // Act
        var result = await _customerController.DeleteCustomer(customerId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal((int)statusCode, objectResult.StatusCode);
    }


    /// <summary>
    /// Returns a list of sample products.
    /// </summary>
    /// <returns>A list of <see cref="Product"/>.</returns>
    private List<Product> GetProductSamples()
    {
        var products = new List<Product>
            {
                new Product { Name = "Laptop" },
                new Product { Name = "Speaker" }
            };

        return products;
    }

    /// <summary>
    /// Returns a list of sample customers.
    /// </summary>
    /// <returns>A list of <see cref="Customer"/>.</returns>
    private List<Customer> GetCustomerSamples()
    {
        var products = GetProductSamples();

        var customers = new List<Customer>
            {
                new Customer
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "1234567890",
                    Address = string.Empty,
                    Sales = new List<Sale>
                    {
                        new Sale { Product = products.First() },
                        new Sale { Product = products.Last() }
                    }
                },
                new Customer
                {
                    FirstName = "Phil",
                    LastName = "Boyce",
                    Email = "phil.boyce@example.com",
                    PhoneNumber = "9876543210",
                    Address = string.Empty,
                    Sales = new List<Sale>
                    {
                        new Sale { Product = products.First() }
                    }
                },
                new Customer
                {
                    FirstName = "Paul",
                    LastName = "Richardson",
                    Email = "customer3@example.com",
                    PhoneNumber = "5555555555",
                    Address = string.Empty,
                    Sales = new List<Sale>
                    {
                        new Sale { Product = products.Last() }
                    }
                },
                new Customer
                {
                    FirstName = "Will",
                    LastName = "Showman",
                    Email = "customer4@example.com",
                    PhoneNumber = "9999999999",
                    Address = string.Empty,
                    Sales = new List<Sale>
                    {
                        new Sale { Product = products.First() },
                        new Sale { Product = products.Last() }
                    }
                },
                new Customer
                {
                    FirstName = "Lara",
                    LastName = "Renze",
                    Email = "customer5@example.com",
                    PhoneNumber = "7777777777",
                    Address = string.Empty,
                    Sales = new List<Sale>
                    {
                        new Sale { Product = products.First() }
                    }
                }
            };

        return customers;
    }
}