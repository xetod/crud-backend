using AutoMapper;
using Crud.Application.Core.AutoMapperProfiles;
using Crud.Application.Core.ResourceParameters;
using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.GetCustomers;
using Crud.Data.Core.PagedLists;
using Crud.Data.Core.Specifications;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities;
using Moq;

namespace Crud.Test.Services;

public class GetCustomersTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetCustomers _getCustomers;
    private readonly IMapper _mapper;

    public GetCustomersTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CustomerProfile>();
            cfg.AddProfile<SaleProfile>();

        });
        _mapper = new Mapper(mapperConfiguration);
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _getCustomers = new GetCustomers(_unitOfWorkMock.Object, _mapper);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsAllRecords()
    {
        // Arrange
        var parameter = new CustomerResourceParameter
        {
            CurrentPage = 1,
            PageSize = 10
        };

        var customers = GetCustomerSamples();

        var pagedList = PagedList<Customer>.Create(customers, parameter.CurrentPage, parameter.PageSize);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer.GetCustomersWithPagination(
                It.IsAny<Specification<Customer>>(),
                parameter.CurrentPage,
                parameter.PageSize))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _getCustomers.ExecuteAsync(parameter);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(5, result.Value.Results.Count);
        Assert.Equal(customers.Count, result.Value.Pagination.TotalCount);
        Assert.Equal(parameter.PageSize, result.Value.Pagination.PageSize);
        Assert.Equal(parameter.CurrentPage, result.Value.Pagination.CurrentPage);

        _unitOfWorkMock.Verify(uow => uow.Customer.GetCustomersWithPagination(
            It.IsAny<Specification<Customer>>(),
            parameter.CurrentPage,
            parameter.PageSize), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsFilteredCustomer()
    {
        // Arrange
        var parameter = new CustomerResourceParameter
        {
            CurrentPage = 1,
            PageSize = 10,
            SearchText = "Customer 1"
        };

        var customers = GetCustomerSamples().Where(customer => customer.FullName == parameter.SearchText).ToList();

        var pagedList = PagedList<Customer>.Create(customers, parameter.CurrentPage, parameter.PageSize);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer.GetCustomersWithPagination(
                It.IsAny<Specification<Customer>>(),
                parameter.CurrentPage,
                parameter.PageSize))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _getCustomers.ExecuteAsync(parameter);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.Results);
        Assert.Equal(customers.Count, result.Value.Pagination.TotalCount);
        Assert.Equal(parameter.PageSize, result.Value.Pagination.PageSize);
        Assert.Equal(parameter.CurrentPage, result.Value.Pagination.CurrentPage);

        _unitOfWorkMock.Verify(uow => uow.Customer.GetCustomersWithPagination(
            It.IsAny<Specification<Customer>>(),
            parameter.CurrentPage,
            parameter.PageSize), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsThreePages()
    {
        // Arrange
        var parameter = new CustomerResourceParameter
        {
            CurrentPage = 1,
            PageSize = 2
        };

        var customers = GetCustomerSamples();

        var pagedList = PagedList<Customer>.Create(customers, parameter.CurrentPage, parameter.PageSize);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer.GetCustomersWithPagination(
                It.IsAny<Specification<Customer>>(),
                parameter.CurrentPage,
                parameter.PageSize))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _getCustomers.ExecuteAsync(parameter);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Results.Count);
        Assert.Equal(5, result.Value.Pagination.TotalCount);
        Assert.Equal(1, result.Value.Pagination.CurrentPage);
        Assert.Equal(3, result.Value.Pagination.TotalPages);

        _unitOfWorkMock.Verify(uow => uow.Customer.GetCustomersWithPagination(
            It.IsAny<Specification<Customer>>(),
            parameter.CurrentPage,
            parameter.PageSize), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsAscendingSortedList()
    {
        // Arrange
        var parameter = new CustomerResourceParameter
        {
            CurrentPage = 1,
            PageSize = 2,
            IsAscending = true
        };

        var customers = GetCustomerSamples();

        var pagedList = PagedList<Customer>.Create(customers, parameter.CurrentPage, parameter.PageSize);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer.GetCustomersWithPagination(
                It.IsAny<Specification<Customer>>(),
                parameter.CurrentPage,
                parameter.PageSize))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _getCustomers.ExecuteAsync(parameter);
        var countOfRows = result.Value.Results.Count();
        var lastRow = result.Value.Results.Skip(countOfRows - 1).First();
        var firstRow = result.Value.Results.First();
        var sorted = string.Compare(firstRow.LastName, lastRow.LastName, StringComparison.Ordinal);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Results.Count);
        Assert.Equal(5, result.Value.Pagination.TotalCount);
        Assert.Equal(1, result.Value.Pagination.CurrentPage);
        Assert.Equal(3, result.Value.Pagination.TotalPages);
        Assert.Equal(-1, sorted);


        _unitOfWorkMock.Verify(uow => uow.Customer.GetCustomersWithPagination(
            It.IsAny<Specification<Customer>>(),
            parameter.CurrentPage,
            parameter.PageSize), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsDescendingSortedList()
    {
        // Arrange
        var parameter = new CustomerResourceParameter
        {
            CurrentPage = 1,
            PageSize = 2,
            IsAscending = false
        };

        var customers = GetCustomerSamples().OrderByDescending(x => x.LastName).ToList();

        var pagedList = PagedList<Customer>.Create(customers, parameter.CurrentPage, parameter.PageSize);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer.GetCustomersWithPagination(
                It.IsAny<Specification<Customer>>(),
                parameter.CurrentPage,
                parameter.PageSize))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _getCustomers.ExecuteAsync(parameter);
        var countOfRows = result.Value.Results.Count();
        var lastRow = result.Value.Results.Skip(countOfRows - 1).First();
        var firstRow = result.Value.Results.First();
        var sorted = string.Compare(firstRow.LastName, lastRow.LastName, StringComparison.Ordinal);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Results.Count);
        Assert.Equal(5, result.Value.Pagination.TotalCount);
        Assert.Equal(1, result.Value.Pagination.CurrentPage);
        Assert.Equal(3, result.Value.Pagination.TotalPages);
        Assert.True(sorted > 0);

        _unitOfWorkMock.Verify(uow => uow.Customer.GetCustomersWithPagination(
            It.IsAny<Specification<Customer>>(),
            parameter.CurrentPage,
            parameter.PageSize), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoRecordsInDb()
    {
        // Arrange
        var parameter = new CustomerResourceParameter
        {
            CurrentPage = 1,
            PageSize = 2
        };

        var customers = new List<Customer>();

        var pagedList = PagedList<Customer>.Create(customers, parameter.CurrentPage, parameter.PageSize);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer.GetCustomersWithPagination(
                It.IsAny<Specification<Customer>>(),
                parameter.CurrentPage,
                parameter.PageSize))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _getCustomers.ExecuteAsync(parameter);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.Results);
        Assert.Equal(0, result.Value.Pagination.TotalCount);
        Assert.Equal(1, result.Value.Pagination.CurrentPage);
        Assert.Equal(0, result.Value.Pagination.TotalPages);

        _unitOfWorkMock.Verify(uow => uow.Customer.GetCustomersWithPagination(
            It.IsAny<Specification<Customer>>(),
            parameter.CurrentPage,
            parameter.PageSize), Times.Once);
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

