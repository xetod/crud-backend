using AutoMapper;
using Crud.Api.Controllers;
using Crud.Application.Core.AutoMapperProfiles;
using Crud.Application.Core.ResourceParameters;
using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.GetCustomer;
using Crud.Application.Services.Customers.GetCustomer.Models;
using Crud.Application.Services.Customers.GetCustomers;
using Crud.Application.Services.Customers.GetCustomers.Models;
using Crud.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Crud.Test.Controllers;

public class CustomerControllerTests
{
    //private readonly CustomerController _customerController;
    //private readonly Mock<IGetCustomers> _getCustomersMock;
    //private readonly Mock<IGetCustomer> _getCustomerMock;
    //private readonly IMapper _mapper;

    //public CustomerControllerTests()
    //{
    //    var mapperConfiguration = new MapperConfiguration(cfg =>
    //    {
    //        cfg.AddProfile<CustomerProfile>();
    //        cfg.AddProfile<SaleProfile>();

    //    });
    //    _mapper = new Mapper(mapperConfiguration);
    //    _getCustomersMock = new Mock<IGetCustomers>();
    //    _getCustomerMock = new Mock<IGetCustomer>();
    //    _customerController = new CustomerController(_getCustomersMock.Object, _getCustomerMock.Object);
    //}

    //// Get Customer
    //[Fact]
    //[Trait("CustomerController", "GetCustomers")]
    //public async Task GetCustomers_WithValidParameter_ReturnsOkResultWithCustomers()
    //{
    //    // Arrange
    //    var parameter = new CustomerResourceParameter();

    //    var customers = GetCustomerSamples();

    //    var paginationMetadata = new PaginationMetadata
    //    {
    //        TotalCount = 5,
    //        PageSize = 2,
    //        CurrentPage = 1,
    //        TotalPages = 3
    //    };

    //    var collectionResource = new CollectionResource<CustomerForListDto>
    //    {
    //        Results = _mapper.Map<List<Customer>, List<CustomerForListDto>>(customers),
    //        Pagination = paginationMetadata
    //    };

    //    var expectedResult = Result.Ok(collectionResource);

    //    _getCustomersMock.Setup(x => x.ExecuteAsync(parameter)).ReturnsAsync(expectedResult);

    //    // Act
    //    var result = await _customerController.GetCustomers(parameter) as OkObjectResult;
    //    var list = result.Value as CollectionResource<CustomerForListDto>;

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(200, result.StatusCode);
    //    Assert.NotNull(list);
    //    Assert.Equal(5, list.Results.Count);
    //    Assert.Equal("Customer 1", $"{list.Results[0].FirstName} {list.Results[0].LastName}");
    //    Assert.Equal("Customer 2", $"{list.Results[1].FirstName} {list.Results[1].LastName}");
    //}



    //// Get Customers
    //[Fact]
    //[Trait("CustomerController", "GetCustomer")]
    //public async Task GetCustomer_CustomerFound_ReturnsOkResultWithData()
    //{
    //    // Arrange
    //    var customerId = 123;
    //    var customerData = new CustomerForDetailDto { CustomerId = customerId, FirstName = "John" };
    //    var successfulResult = Result.Ok<CustomerForDetailDto>(customerData);

    //    _getCustomerMock.Setup(mock => mock.ExecuteAsync(customerId)).ReturnsAsync(successfulResult);

    //    // Act
    //    var result = await _customerController.GetCustomer(customerId);

    //    // Assert
    //    Assert.IsType<OkObjectResult>(result);
    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    Assert.Equal(customerData, okResult.Value);
    //}

    //[Fact]
    //[Trait("CustomerController", "GetCustomer")]
    //public async Task GetCustomer_CustomerNotFound_ReturnsStatusCodeWithResultObject()
    //{
    //    // Arrange
    //    var customerId = 123;
    //    var errorMessage = "Customer not found.";
    //    var statusCode = HttpStatusCode.NotFound;
    //    var failedResult = Result.Fail<CustomerForDetailDto>(errorMessage);
        
    //    _getCustomerMock.Setup(mock => mock.ExecuteAsync(customerId)).ReturnsAsync(failedResult);

    //    // Act
    //    var result = await _customerController.GetCustomer(customerId);

    //    // Assert
    //    Assert.IsType<ObjectResult>(result);
    //    var objectResult = Assert.IsType<ObjectResult>(result);
    //    Assert.Equal((int)failedResult.StatusCode, objectResult.StatusCode);
    //    Assert.Equal(failedResult, objectResult.Value);
    //}



    //private List<Product> GetProductSamples()
    //{
    //    var products = new List<Product>
    //        {
    //            new Product { Name = "Product 1" },
    //            new Product { Name = "Product 2" }
    //        };

    //    return products;
    //}

    //private List<Customer> GetCustomerSamples()
    //{
    //    var products = GetProductSamples();

    //    var customers = new List<Customer>
    //        {
    //             new Customer
    //            {
    //                FirstName = "Customer",
    //                LastName = "1",
    //                Email = "customer1@example.com",
    //                PhoneNumber = "1234567890",
    //                Address = string.Empty,
    //                Sales = new List<Sale>
    //                {
    //                    new Sale { Product = products.First() },
    //                    new Sale { Product = products.Last() }
    //                }
    //            },
    //            new Customer
    //             {
    //                 FirstName = "Customer",
    //                 LastName = "2",
    //                 Email = "customer2@example.com",
    //                 PhoneNumber = "9876543210",
    //                 Address = string.Empty,
    //                 Sales = new List<Sale>
    //                 {
    //                     new Sale { Product = products.First() }
    //                 }
    //             },
    //            new Customer
    //            {
    //                FirstName = "Customer",
    //                LastName = "3",
    //                Email = "customer3@example.com",
    //                PhoneNumber = "5555555555",
    //                Address = string.Empty,
    //                Sales = new List<Sale>
    //                    {
    //                        new Sale { Product = products.Last() }
    //                    }
    //            },
    //            new Customer
    //            {
    //                FirstName = "Customer",
    //                LastName = "4",
    //                Email = "customer4@example.com",
    //                PhoneNumber = "9999999999",
    //                Address = string.Empty,
    //                Sales = new List<Sale>
    //                    {
    //                        new Sale { Product = products.First() },
    //                        new Sale { Product = products.Last() }
    //                    }
    //            },
    //            new Customer
    //            {
    //                FirstName = "Customer",
    //                LastName = "5",
    //                Email = "customer5@example.com",
    //                PhoneNumber = "7777777777",
    //                Address = string.Empty,
    //                Sales = new List<Sale>
    //                    {
    //                        new Sale { Product = products.First() }
    //                    }
    //            }
    //        };

    //    return customers;
    //}
}