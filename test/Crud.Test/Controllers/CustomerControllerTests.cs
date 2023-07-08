using AutoMapper;
using Crud.Api.Controllers;
using Crud.Application.Core.AutoMapperProfiles;
using Crud.Application.Core.ResourceParameters;
using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.GetCustomers;
using Crud.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Crud.Test.Controllers;

public class CustomerControllerTests
{
    private readonly CustomerController _customerController;
    private readonly Mock<IGetCustomers> _getCustomersMock;
    private readonly IMapper _mapper;

    public CustomerControllerTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CustomerProfile>();
            cfg.AddProfile<SaleProfile>();

        });
        _mapper = new Mapper(mapperConfiguration);
        _getCustomersMock = new Mock<IGetCustomers>();
        _customerController = new CustomerController(_getCustomersMock.Object);
    }

    [Fact]
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

        _getCustomersMock.Setup(x => x.ExecuteAsync(parameter)).ReturnsAsync(expectedResult);

        // Act
        var result = await _customerController.GetCustomers(parameter) as OkObjectResult;
        var list = result.Value as CollectionResource<CustomerForListDto>;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(list);
        Assert.Equal(5, list.Results.Count);
        Assert.Equal("Customer 1", $"{list.Results[0].FirstName} {list.Results[0].LastName}");
        Assert.Equal("Customer 2", $"{list.Results[1].FirstName} {list.Results[1].LastName}");
    }

    private List<Product> GetProductSamples()
    {
        var products = new List<Product>
            {
                new Product { Name = "Product 1" },
                new Product { Name = "Product 2" }
            };

        return products;
    }

    private List<Customer> GetCustomerSamples()
    {
        var products = GetProductSamples();

        var customers = new List<Customer>
            {
                 new Customer
                {
                    FirstName = "Customer",
                    LastName = "1",
                    Email = "customer1@example.com",
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
                     FirstName = "Customer",
                     LastName = "2",
                     Email = "customer2@example.com",
                     PhoneNumber = "9876543210",
                     Address = string.Empty,
                     Sales = new List<Sale>
                     {
                         new Sale { Product = products.First() }
                     }
                 },
                new Customer
                {
                    FirstName = "Customer",
                    LastName = "3",
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
                    FirstName = "Customer",
                    LastName = "4",
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
                    FirstName = "Customer",
                    LastName = "5",
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