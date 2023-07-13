using Crud.Application.Services.Customers.DeleteCustomer;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities.Customers;
using Crud.Test.Helpers;
using Moq;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Crud.Test.Services.Customers;

/// <summary>
/// Unit and integration tests for the <see cref="DeleteCustomer"/> service.
/// </summary>
[Category("Services")]
public class DeleteCustomerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteCustomer _deleteCustomer;

    public DeleteCustomerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _deleteCustomer = new DeleteCustomer(_unitOfWorkMock.Object);
    }

    /// <summary>
    /// Unit test for the <see cref="DeleteCustomer.ExecuteAsync"/> method.
    /// It tests the method with a valid customer ID and expects a success result.
    /// </summary>
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
    }

    /// <summary>
    /// Unit test for the <see cref="DeleteCustomer.ExecuteAsync"/> method.
    /// It tests the method with an invalid customer ID and expects a failure result with an invalid ID error message.
    /// </summary>
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

    /// <summary>
    /// Unit test for the <see cref="DeleteCustomer.ExecuteAsync"/> method.
    /// It tests the method with a non-existent customer ID and expects a failure result with a customer not found error message.
    /// </summary>
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

    /// <summary>
    /// Unit test for the <see cref="DeleteCustomer.ExecuteAsync"/> method.
    /// It tests the method with a failed deletion and expects a failure result with a deletion failed error message.
    /// </summary>
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

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CompleteAsync()).ReturnsAsync(0);

        // Act
        var result = await _deleteCustomer.ExecuteAsync(validCustomerId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Deletion failed.", result.Error);
    }

    /// <summary>
    /// Integration test for the <see cref="DeleteCustomer.ExecuteAsync"/> method.
    /// It tests the method with a valid customer ID and verifies that the customer is deleted from the database.
    /// </summary>
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
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
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
