using AutoMapper;
using Crud.Application.Core.AutoMapperProfiles.Customers;
using Crud.Application.Core.AutoMapperProfiles.Sales;
using Crud.Application.Services.Customers.GetCustomers;
using Crud.Data.Core.PagedLists;
using Crud.Data.Core.Specifications;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities;
using Moq;
using System.ComponentModel;

namespace Crud.Test.Services.Customers;

/// <summary>
/// Unit tests for the <see cref="GetCustomers"/> service.
/// </summary>
[Category("Services")]
public class GetCustomersTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetCustomers _getCustomers;

    public GetCustomersTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CustomerProfile>();
            cfg.AddProfile<SaleProfile>();
        });
        IMapper mapper = new Mapper(mapperConfiguration);
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _getCustomers = new GetCustomers(_unitOfWorkMock.Object, mapper);
    }

    /// <summary>
    /// Unit test for the <see cref="GetCustomers.ExecuteAsync"/> method.
    /// It verifies that all customer records are returned without filtering.
    /// </summary>
    [Fact]
    [Trait("Services", "GetCustomers")]
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

    /// <summary>
    /// Unit test for the <see cref="GetCustomers.ExecuteAsync"/> method.
    /// It verifies that filtered customers are returned based on the search text.
    /// </summary>
    [Fact]
    [Trait("Services", "GetCustomers")]
    public async Task ExecuteAsync_ReturnsFilteredCustomer()
    {
        // Arrange
        var parameter = new CustomerResourceParameter
        {
            CurrentPage = 1,
            PageSize = 10,
            SearchText = "Jane"
        };

        var customers = GetCustomerSamples().Where(customer => customer.FullName.Contains(parameter.SearchText)).ToList();

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

    /// <summary>
    /// Unit test for the <see cref="GetCustomers.ExecuteAsync"/> method.
    /// It verifies that the correct number of pages is returned based on the page size.
    /// </summary>
    [Fact]
    [Trait("Services", "GetCustomers")]
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

    /// <summary>
    /// Unit test for the <see cref="GetCustomers.ExecuteAsync"/> method.
    /// It verifies that the returned list is sorted in ascending order based on the last name.
    /// </summary>
    [Fact]
    [Trait("Services", "GetCustomers")]
    public async Task ExecuteAsync_ReturnsAscendingSortedList()
    {
        // Arrange
        var parameter = new CustomerResourceParameter
        {
            CurrentPage = 1,
            PageSize = 2,
            IsAscending = true
        };

        var customers = GetCustomerSamples().OrderBy(x => x.LastName).ToList();

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
        Assert.True(sorted < 0);

        _unitOfWorkMock.Verify(uow => uow.Customer.GetCustomersWithPagination(
            It.IsAny<Specification<Customer>>(),
            parameter.CurrentPage,
            parameter.PageSize), Times.Once);
    }

    /// <summary>
    /// Unit test for the <see cref="GetCustomers.ExecuteAsync"/> method.
    /// It verifies that the returned list is sorted in descending order based on the last name.
    /// </summary>
    [Fact]
    [Trait("Services", "GetCustomers")]
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

    /// <summary>
    /// Unit test for the <see cref="GetCustomers.ExecuteAsync"/> method.
    /// It verifies that an empty list is returned when there are no records in the database.
    /// </summary>
    [Fact]
    [Trait("Services", "GetCustomers")]
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
                new Product { Name = "Laptop" },
                new Product { Name = "Speaker" }
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


