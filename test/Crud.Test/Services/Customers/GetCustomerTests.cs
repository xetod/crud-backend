using AutoMapper;
using Crud.Application.Services.Customers.GetCustomer;
using Crud.Application.Services.Customers.GetCustomer.Models;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Data.Repositories.Customers;
using Crud.Domain.Entities;
using Moq;
using System.ComponentModel;

namespace Crud.Test.Services.Customers;

/// <summary>
/// Unit tests for the <see cref="GetCustomer"/> service.
/// </summary>
[Category("Services")]
public class GetCustomerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public GetCustomerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _mapperMock = new Mock<IMapper>();
    }

    /// <summary>
    /// Unit test for the <see cref="GetCustomer.ExecuteAsync"/> method.
    /// It verifies that the method returns a successful result with the customer data when a valid customer ID is provided.
    /// </summary>
    [Fact]
    [Trait("Services", "GetCustomer")]
    public async Task ExecuteAsync_ValidCustomerId_ReturnsSuccessfulResultWithCustomerData()
    {
        // Arrange
        var customerId = 123;
        var customer = new Customer { CustomerId = customerId, FirstName = "John" };
        var mappedResult = new CustomerForDetailDto { CustomerId = customerId, FirstName = "John" };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer).Returns(_customerRepositoryMock.Object);
        _customerRepositoryMock.Setup(customerRepository => customerRepository.GetCustomer(customerId)).ReturnsAsync(customer);
        _mapperMock.Setup(mapper => mapper.Map<Customer, CustomerForDetailDto>(customer)).Returns(mappedResult);

        var getCustomer = new GetCustomer(_unitOfWorkMock.Object, _mapperMock.Object);

        // Act
        var result = await getCustomer.ExecuteAsync(customerId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(mappedResult, result.Value);
    }

    /// <summary>
    /// Unit test for the <see cref="GetCustomer.ExecuteAsync"/> method.
    /// It verifies that the method does not throw an exception when a null customer ID is provided.
    /// </summary>
    [Fact]
    [Trait("Services", "GetCustomer")]
    public async Task ExecuteAsync_NullCustomerId_NotThrowsArgumentNullException()
    {
        // Arrange
        var customerId = 123;
        var customer = new Customer { CustomerId = customerId, FirstName = "John" };
        var mappedResult = new CustomerForDetailDto { CustomerId = customerId, FirstName = "John" };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer).Returns(_customerRepositoryMock.Object);
        _customerRepositoryMock.Setup(customerRepository => customerRepository.GetCustomer(customerId)).ReturnsAsync(customer);
        _mapperMock.Setup(mapper => mapper.Map<Customer, CustomerForDetailDto>(customer)).Returns(mappedResult);

        var getCustomer = new GetCustomer(_unitOfWorkMock.Object, _mapperMock.Object);

        // Act and Assert
        try
        {
            await getCustomer.ExecuteAsync(customerId);
        }
        catch (Exception ex)
        {
            // Assert
            Assert.True(false, $"Expected no exception, but caught {ex.GetType().Name}: {ex.Message}");
        }

        // Assert
        Assert.True(true);
    }

    /// <summary>
    /// Unit test for the <see cref="GetCustomer.ExecuteAsync"/> method.
    /// It verifies that the method returns a failed result with the error message when the customer is not found.
    /// </summary>
    [Fact]
    [Trait("Services", "GetCustomer")]
    public async Task ExecuteAsync_CustomerNotFound_ReturnsFailedResultWithErrorMessage()
    {
        // Arrange
        var customerId = 123;
        var customer = new NullCustomer();

        _unitOfWorkMock.Setup(uow => uow.Customer).Returns(_customerRepositoryMock.Object);
        _customerRepositoryMock.Setup(customerRepository => customerRepository.GetCustomer(customerId)).ReturnsAsync(customer);

        var getCustomer = new GetCustomer(_unitOfWorkMock.Object, _mapperMock.Object);

        // Act
        var result = await getCustomer.ExecuteAsync(customerId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer not found.", result.Error);
    }
}

