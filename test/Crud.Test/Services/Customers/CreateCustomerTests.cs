using Crud.Application.Services.Customers.CreateCustomer;
using Crud.Application.Services.Customers.CreateCustomer.Factory;
using Crud.Application.Services.Customers.CreateCustomer.Models;
using Crud.Application.Services.Customers.CreateCustomer.Validation;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Data.Repositories.Customers;
using Crud.Domain.Entities;
using Crud.Test.Helpers;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Crud.Test.Services.Customers
{
    public class CreateCustomerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<ICustomerFactory> _customerFactoryMock;
        private readonly CreateCustomer _createCustomer;
        private readonly CustomerForCreationValidator _customerForCreationValidator;
        private readonly Mock<IValidator<Customer>> _validatorMock;

        public CreateCustomerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerFactoryMock = new Mock<ICustomerFactory>();
            _customerForCreationValidator = new CustomerForCreationValidator();
            _validatorMock = new Mock<IValidator<Customer>>();

            _createCustomer = new CreateCustomer(
                _unitOfWorkMock.Object,
                _customerFactoryMock.Object,
                _customerForCreationValidator
            );
        }

        // Customer Factory
        [Fact]
        [Trait("Services", "CustomerFactory")]
        public void Create_ReturnsNewCustomerWithSpecifiedDetails()
        {
            // Arrange
            var factory = new CustomerFactory();
            var firstName = "Jane";
            var lastName = "Smith";
            var email = "jane.smith@example.com";
            var address = "456 Oak St";
            var phoneNumber = "555-5678";
            var sales = new List<SaleForCreateDto>
            {
                new SaleForCreateDto { ProductId = 1, Quantity = 2, UnitPrice = 10.0m },
                new SaleForCreateDto { ProductId = 2, Quantity = 3, UnitPrice = 15.0m },
                new SaleForCreateDto { ProductId = 3, Quantity = 1, UnitPrice = 20.0m}
            };

            // Act
            var customer = factory.Create(firstName, lastName, email, address, phoneNumber, sales);

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

        [Fact]
        [Trait("Services", "CustomerFactory")]
        public void Create_ReturnsNewCustomerWithEmptySalesListWhenSalesIsNull()
        {
            // Arrange
            var factory = new CustomerFactory();
            var firstName = "Jane";
            var lastName = "Smith";
            var email = "jane.smith@example.com";
            var address = "456 Oak St";
            var phoneNumber = "555-5678";

            // Act
            var customer = factory.Create(firstName, lastName, email, address, phoneNumber, null);

            // Assert
            Assert.Empty(customer.Sales);
        }



        // Create Customer
        [Fact]
        [Trait("Services", "CreateCustomer")]
        public void ExecuteAsync_WithInvalidModel_InvokesValidator()
        {
            // Arrange
            var customer = new CustomerForCreateDto
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                Address = string.Empty,
                PhoneNumber = string.Empty,
                Sales = new List<SaleForCreateDto>
                {
                    new SaleForCreateDto { ProductId = 1, Quantity = 2, UnitPrice = 0.0m },
                    new SaleForCreateDto { ProductId = 2, Quantity = 3, UnitPrice = 0.0m },
                    new SaleForCreateDto { ProductId = 3, Quantity = 1, UnitPrice = 0.0m}
                }
            };

            // Act
            var validationResult = _customerForCreationValidator.TestValidate(customer);

            // Assert
            validationResult.ShouldHaveValidationErrorFor(m => m.FirstName);
            validationResult.ShouldHaveValidationErrorFor(m => m.LastName);
            validationResult.ShouldHaveValidationErrorFor(m => m.Email);
            validationResult.ShouldHaveValidationErrorFor(m => m.Address);
            validationResult.ShouldHaveValidationErrorFor(m => m.PhoneNumber);
            validationResult.ShouldHaveValidationErrorFor("Sales[0].UnitPrice");
        }

        [Fact]
        [Trait("Services", "CreateCustomer")]
        public async Task ExecuteAsync_WithInvalidModel_ReturnsFailResult()
        {
            // Arrange
            var model = new CustomerForCreateDto();

            // Act
            var result = await _createCustomer.ExecuteAsync(model);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Customer model is not valid.", result.Error);
        }

        [Fact]
        [Trait("Services", "CreateCustomer")]
        public async Task ExecuteAsync_CompletingUnitOfWorkFails_ReturnsFailResult()
        {
            // Arrange
            var model = new CustomerForCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                PhoneNumber = "555-1234",
                Sales = new List<SaleForCreateDto>
                {
                    new SaleForCreateDto { ProductId = 2, Quantity = 3, UnitPrice = 10.0m},
                }
            };

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Sales = model.Sales.Select(dto => new Sale
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice
                }).ToList()
            };

            _customerFactoryMock.Setup(f => f.Create(
                model.FirstName,
                model.LastName,
                model.Email,
                model.Address,
                model.PhoneNumber,
                model.Sales
            )).Returns(customer);

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer).Returns(_customerRepositoryMock.Object);

            _unitOfWorkMock.Setup(uow => uow.Customer.AddAsync(customer)).Verifiable();

            _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).ReturnsAsync(0);

            _customerRepositoryMock.Setup(customerRepository => customerRepository.AddAsync(customer)).Returns(Task.CompletedTask);

            var createCustomer = new CreateCustomer(_unitOfWorkMock.Object, _customerFactoryMock.Object, _customerForCreationValidator);

            // Act
            var result = await createCustomer.ExecuteAsync(model);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Failed to save customer.", result.Error);
            _unitOfWorkMock.Verify(uow => uow.Customer.AddAsync(customer), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        [Trait("Services", "CreateCustomer")]
        public async Task ExecuteAsync_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var model = new CustomerForCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                PhoneNumber = "555-1234",
                Sales = new List<SaleForCreateDto>
                {
                    new SaleForCreateDto { ProductId = 2, Quantity = 3, UnitPrice = 10.0m}
                }
            };

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Sales = model.Sales.Select(dto => new Sale
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice
                }).ToList()
            };

            _customerFactoryMock.Setup(f => f.Create(
                model.FirstName,
                model.LastName,
                model.Email,
                model.Address,
                model.PhoneNumber,
                model.Sales
            )).Returns(customer);

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Customer).Returns(_customerRepositoryMock.Object);

            _unitOfWorkMock.Setup(uow => uow.Customer.AddAsync(customer)).Verifiable();

            _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            _customerRepositoryMock.Setup(customerRepository => customerRepository.AddAsync(customer)).Returns(Task.CompletedTask);

            var createCustomer = new CreateCustomer(_unitOfWorkMock.Object, _customerFactoryMock.Object, _customerForCreationValidator);

            // Act
            var result = await createCustomer.ExecuteAsync(model);

            // Assert
            Assert.True(result.Success);
            _unitOfWorkMock.Verify(uow => uow.Customer.AddAsync(customer), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        [Trait("Services", "CreateCustomer")]
        public async Task ExecuteAsync_ValidModel_ReturnsTheCustomer()
        {
            // Arrange
            using var factory = new CrudDbContextFactory();
            await using var dbContext = factory.CreateContext();

            // Create and seed test data
            dbContext.Product.AddRange(GetProductSamples());
            await dbContext.SaveChangesAsync();

            var model = new CustomerForCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                PhoneNumber = "555-1234",
                Sales = new List<SaleForCreateDto>
                {
                    new SaleForCreateDto { ProductId = 2, Quantity = 3, UnitPrice = 10.0m},
                }
            };

            var unitOfWork = new UnitOfWork(dbContext);
            var customerFactory = new CustomerFactory();
            var customerValidator = new CustomerForCreationValidator();
            var createCustomer = new CreateCustomer(unitOfWork, customerFactory, customerValidator);

            // Act
            var result = await createCustomer.ExecuteAsync(model);

            var customer = dbContext.Customer.FirstOrDefault();

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(customer);
            Assert.NotEmpty(customer.FirstName);
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
    }
}
