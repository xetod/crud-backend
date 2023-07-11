using Crud.Application.Services.Customers.CreateCustomer.Factory;
using Crud.Application.Services.Customers.UpdateCustomer;
using Crud.Application.Services.Customers.UpdateCustomer.Models;
using Crud.Application.Services.Customers.UpdateCustomer.Validation;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Data.Repositories.Customers;
using Crud.Domain.Entities;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using System.ComponentModel;
using System.Net;

namespace Crud.Test.Services.Customers;

/// <summary>
/// Unit tests for the <see cref="UpdateCustomer"/> service.
/// </summary>
[Category("Services")]
public class UpdateCustomerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<ICustomerFactory> _customerFactoryMock;
    private readonly UpdateCustomer _updateCustomer;
    private readonly CustomerForUpdateValidator _customerForUpdateValidator;
    private readonly Mock<IValidator<Customer>> _validatorMock;

    public UpdateCustomerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerFactoryMock = new Mock<ICustomerFactory>();
        _customerForUpdateValidator = new CustomerForUpdateValidator();
        _validatorMock = new Mock<IValidator<Customer>>();

        _updateCustomer = new UpdateCustomer(
            _unitOfWorkMock.Object,
            _customerFactoryMock.Object,
            _customerForUpdateValidator
        );
    }

    // Customer Factory

    /// <summary>
    /// Unit test for the <see cref="CustomerFactory.Create"/> method.
    /// It verifies that a new customer is created with the specified details and sales data.
    /// </summary>
    [Fact]
    [Trait("Services", "CustomerFactory")]
    public void Update_ReturnsNewCustomerWithSpecifiedDetails()
    {
        // Arrange
        var factory = new CustomerFactory();
        var customerId = 1;
        var firstName = "Jane";
        var lastName = "Smith";
        var email = "jane.smith@example.com";
        var address = "456 Oak St";
        var phoneNumber = "555-5678";
        var sales = new List<SaleForUpdateDto>
            {
                new SaleForUpdateDto { ProductId = 1, Quantity = 2, UnitPrice = 10.0m },
                new SaleForUpdateDto { ProductId = 2, Quantity = 3, UnitPrice = 15.0m },
                new SaleForUpdateDto { ProductId = 3, Quantity = 1, UnitPrice = 20.0m}
            };

        // Act
        var customer = factory.Create(customerId, firstName, lastName, email, address, phoneNumber, sales);

        // Assert
        Assert.Equal(firstName, customer.FirstName);
        Assert.Equal(lastName, customer.LastName);
        Assert.Equal(email, customer.Email);
        Assert.Equal(address, customer.Address);
        Assert.Equal(phoneNumber, customer.PhoneNumber);
        Assert.Equal(sales.Count, customer.Sales.Count);
        Assert.All(sales, dto =>
        {
            var sale = customer.Sales.FirstOrDefault(s => s.ProductId == dto.ProductId);
            Assert.NotNull(sale);
            Assert.Equal(dto.Quantity, sale.Quantity);
            Assert.Equal(dto.UnitPrice, sale.UnitPrice);
            Assert.Equal(DateTime.Now.Date, sale.Date.Date);
        });
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerFactory.Create"/> method.
    /// It verifies that a new customer is created with empty sales list when sales parameter is null.
    /// </summary>
    [Fact]
    [Trait("Services", "CustomerFactory")]
    public void Create_ReturnsNewCustomerWithEmptySalesListWhenSalesIsNull()
    {
        // Arrange
        var factory = new CustomerFactory();
        var customerId = 1;
        var firstName = "Jane";
        var lastName = "Smith";
        var email = "jane.smith@example.com";
        var address = "456 Oak St";
        var phoneNumber = "555-5678";
        var sales = new List<SaleForUpdateDto>
            {
                new SaleForUpdateDto { ProductId = 1, Quantity = 2, UnitPrice = 10.0m },
                new SaleForUpdateDto { ProductId = 2, Quantity = 3, UnitPrice = 15.0m },
                new SaleForUpdateDto { ProductId = 3, Quantity = 1, UnitPrice = 20.0m}
            };

        // Act
        var customer = factory.Create(customerId, firstName, lastName, email, address, phoneNumber, null);

        // Assert
        Assert.Empty(customer.Sales);
    }

    // Update Customer

    /// <summary>
    /// Unit test for the <see cref="UpdateCustomer.ExecuteAsync"/> method.
    /// It verifies that the validator is invoked when an invalid model is passed.
    /// </summary>
    [Fact]
    [Trait("Services", "UpdateCustomer")]
    public void ExecuteAsync_WithInvalidModel_InvokesValidator()
    {
        // Arrange
        var customer = new CustomerForUpdateDto
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            Email = string.Empty,
            Address = string.Empty,
            PhoneNumber = string.Empty,
            Sales = new List<SaleForUpdateDto>
                {
                    new SaleForUpdateDto { ProductId = 1, Quantity = 2, UnitPrice = 0.0m },
                    new SaleForUpdateDto { ProductId = 2, Quantity = 3, UnitPrice = 0.0m },
                    new SaleForUpdateDto { ProductId = 3, Quantity = 1, UnitPrice = 0.0m}
                }
        };

        // Act
        var validationResult = _customerForUpdateValidator.TestValidate(customer);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(m => m.FirstName);
        validationResult.ShouldHaveValidationErrorFor(m => m.LastName);
        validationResult.ShouldHaveValidationErrorFor(m => m.Email);
        validationResult.ShouldHaveValidationErrorFor(m => m.Address);
        validationResult.ShouldHaveValidationErrorFor(m => m.PhoneNumber);
        validationResult.ShouldHaveValidationErrorFor("Sales[0].UnitPrice");
    }

    /// <summary>
    /// Unit test for the <see cref="UpdateCustomer.ExecuteAsync"/> method.
    /// It verifies that the method returns a failed result when an invalid model is passed.
    /// </summary>
    [Fact]
    [Trait("Services", "UpdateCustomer")]
    public async Task ExecuteAsync_WithInvalidModel_ReturnsFailResult()
    {
        // Arrange
        var model = new CustomerForUpdateDto();

        // Act
        var result = await _updateCustomer.ExecuteAsync(model);

        // Assert
        Assert.False(result.Success);
    }

    /// <summary>
    /// Unit test for the <see cref="UpdateCustomer.ExecuteAsync"/> method.
    /// It verifies that the method returns a successful result and updates the customer when a valid model is passed.
    /// </summary>
    [Fact]
    [Trait("Services", "UpdateCustomer")]
    public async Task ExecuteAsync_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var model = new CustomerForUpdateDto
        {
            CustomerId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = "123 Main St",
            PhoneNumber = "555-1234",
            Sales = new List<SaleForUpdateDto>
                {
                    new SaleForUpdateDto { ProductId = 2, Quantity = 3, UnitPrice = 10.0m},
                }
        };

        var customerFromDb = new Customer();

        _unitOfWorkMock.Setup(uow => uow.Customer.GetCustomer(model.CustomerId)).ReturnsAsync(customerFromDb);
        _unitOfWorkMock.Setup(uow => uow.Customer.Update(It.IsAny<Customer>(), customerFromDb)).Verifiable();
        _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

        // Act
        var result = await _updateCustomer.ExecuteAsync(model);

        // Assert
        Assert.True(result.Success);
        _unitOfWorkMock.Verify(uow => uow.Customer.Update(It.IsAny<Customer>(), customerFromDb), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
    }

    /// <summary>
    /// Unit test for the <see cref="UpdateCustomer.ExecuteAsync"/> method.
    /// It verifies that the method returns a failure result when the customer does not exist.
    /// </summary>
    [Fact]
    [Trait("Services", "UpdateCustomer")]
    public async Task ExecuteAsync_WithNonExistingCustomer_ReturnsFailureResult()
    {
        // Arrange
        var model = new CustomerForUpdateDto
        {
            CustomerId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = "123 Main St",
            PhoneNumber = "555-1234",
            Sales = new List<SaleForUpdateDto>
                {
                    new SaleForUpdateDto { ProductId = 2, Quantity = 3, UnitPrice = 10.0m},
                }
        };

        _unitOfWorkMock.Setup(uow => uow.Customer.GetCustomer(model.CustomerId)).ReturnsAsync((Customer)null);

        // Act
        var result = await _updateCustomer.ExecuteAsync(model);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Customer not found.", result.Error);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}

