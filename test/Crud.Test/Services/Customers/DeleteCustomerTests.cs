using Crud.Application.Services.Customers.DeleteCustomer;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities;
using Crud.Test.Helpers;
using Moq;
using System.Linq.Expressions;

namespace Crud.Test.Services.Customers;

public class DeleteCustomerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteCustomer _deleteCustomer;

    public DeleteCustomerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _deleteCustomer = new DeleteCustomer(_unitOfWorkMock.Object);
    }

    [Fact]
    [Trait("Services", "DeleteCustomer")]
    public async Task ExecuteAsync_WithValidCustomerId_ReturnsSuccessResult()
    {
        // Arrange
        var customerId = 1;
        var customer = new Customer { CustomerId = customerId };
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Customer.FirstOrDefaultAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customer);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CompleteAsync()).ReturnsAsync(1);

        // Act
        var result = await _deleteCustomer.ExecuteAsync(customerId);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Value);
    }

    [Fact]
    [Trait("Services", "DeleteCustomer")]
    public async Task ExecuteAsync_WithInvalidCustomerId_ReturnsFailureResult_WithInvalidIdErrorMessage()
    {
        // Arrange
        int invalidCustomerId = default;

        // Act
        var result = await _deleteCustomer.ExecuteAsync(invalidCustomerId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer ID is invalid.", result.Error);
    }

    [Fact]
    [Trait("Services", "DeleteCustomer")]
    public async Task ExecuteAsync_WithNonExistentCustomerId_ReturnsFailureResult_WithCustomerNotFoundErrorMessage()
    {
        // Arrange
        var nonExistentCustomerId = 1;
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Customer.FirstOrDefaultAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync((Customer)null);

        // Act
        var result = await _deleteCustomer.ExecuteAsync(nonExistentCustomerId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer not found.", result.Error);
    }

    [Fact]
    [Trait("Services", "DeleteCustomer")]
    public async Task ExecuteAsync_WithFailedDeletion_ReturnsFailureResult_WithDeletionFailedErrorMessage()
    {
        // Arrange
        var validCustomerId = 1;
        var customer = new Customer { CustomerId = validCustomerId };
        _unitOfWorkMock
            .Setup(uow => uow.Customer.FirstOrDefaultAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customer);
        _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).ReturnsAsync(0);

        // Act
        var result = await _deleteCustomer.ExecuteAsync(validCustomerId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Deletion failed.", result.Error);
    }

    [Fact]
    [Trait("Services", "DeleteCustomer")]
    public async Task ExecuteAsync_WithValidCustomerId_DeletesCustomerFromDatabase()
    {
        // Arrange
        var validCustomerId = 1;
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        // Create and seed test data
        var customer = new Customer
        {
            FirstName = "Customer",
            LastName = "1",
            Email = "customer1@example.com",
            PhoneNumber = "1234567890",
            Address = "address 1"
        };

        dbContext.Customer.AddRange(customer);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        var unitOfWork = new UnitOfWork(dbContext);
        var sut = new DeleteCustomer(unitOfWork);

        // Act
        var result = await sut.ExecuteAsync(validCustomerId);

        // Assert
        Assert.True(result.Success);
    }
}
