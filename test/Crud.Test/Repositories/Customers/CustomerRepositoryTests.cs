﻿using Crud.Application.Services.Customers.GetCustomers;
using Crud.Application.Services.Customers.GetCustomers.Specifications;
using Crud.Data.Core.Specifications;
using Crud.Data.Repositories.Customers;
using Crud.Domain.Entities.Customers;
using Crud.Domain.Entities.Products;
using Crud.Domain.Entities.Sales;
using Crud.Test.Helpers;
using System.ComponentModel;

namespace Crud.Test.Repositories.Customers;

/// <summary>
/// Integration tests for the <see cref="CustomerRepository"/> class.
/// </summary>
[Category("Repositories")]
public class CustomerRepositoryTests
{
    //Get Customer

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomer"/> method.
    /// It verifies that a customer with sales and products is returned when the customer exists in the database.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomer")]
    public async Task GetCustomer_ReturnCustomerWithSalesAndProducts()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        // Create and seed test data
        dbContext.Product.AddRange(GetProductSamples());
        dbContext.Customer.AddRange(GetCustomerSamples());
        await dbContext.SaveChangesAsync();

        var sut = new CustomerRepository(dbContext);

        // Act
        var result = await sut.GetCustomer(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("customer1@example.com", result.Email);
        Assert.Equal("1234567890", result.PhoneNumber);
        Assert.Equal(2, result.Sales.Count);
        Assert.Contains(result.Sales, sale => sale.Product.Name == "Product 1");
        Assert.Contains(result.Sales, sale => sale.Product.Name == "Product 2");
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomer"/> method.
    /// It verifies that a customer with no sales is returned when the customer exists in the database.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomer")]
    public async Task GetCustomer_ReturnsCustomerWithNoSales()
    {
        // Arrange
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

        var sut = new CustomerRepository(dbContext);

        // Act
        var result = await sut.GetCustomer(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("customer1@example.com", result.Email);
        Assert.Equal("1234567890", result.PhoneNumber);
        Assert.Empty(result.Sales);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomer"/> method.
    /// It verifies that an empty customer is returned when the customer is not found in the database.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomer")]
    public async Task GetCustomer_ReturnsEmptyWhenCustomerNotFound()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        var sut = new CustomerRepository(dbContext);

        // Act
        var result = await sut.GetCustomer(1);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.FirstName);
        Assert.Empty(result.LastName);
        Assert.Equal(-1, result.CustomerId);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomer"/> method.
    /// It verifies that no exception is thrown when the customer is not found in the database.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomer")]
    public async Task GetCustomer_ThrowsNoExceptionWhenCustomerNotFound()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();
        var sut = new CustomerRepository(dbContext);

        // Act and Assert
        try
        {
            await sut.GetCustomer(1);
        }
        catch (Exception ex)
        {
            // Assert
            Assert.True(false, $"Expected no exception, but caught {ex.GetType().Name}: {ex.Message}");
        }

        // Assert
        Assert.True(true);
    }



    //Get Customers With Pagination

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomersWithPagination"/> method.
    /// It verifies that three pages of customers are returned when pagination is applied.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomersWithPagination")]
    public async Task GetCustomersWithPagination_ReturnsThreePages()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        // Create and seed test data
        dbContext.Product.AddRange(GetProductSamples());
        dbContext.Customer.AddRange(GetCustomerSamples());
        await dbContext.SaveChangesAsync();

        var sut = new CustomerRepository(dbContext);
        var specification = CreateSpecification(true);

        // Act
        var result = await sut.GetCustomersWithPagination(specification, currentPage: 1, pageSize: 2);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(3, result.TotalPages);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomersWithPagination"/> method.
    /// It verifies that a sorted ascending list of customers is returned.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomersWithPagination")]
    public async Task GetCustomersWithPagination_ReturnsAscendingSortedList()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        // Create and seed test data
        dbContext.Product.AddRange(GetProductSamples());
        dbContext.Customer.AddRange(GetCustomerSamples());
        await dbContext.SaveChangesAsync();

        var sut = new CustomerRepository(dbContext);
        var specification = CreateSpecification(true);

        // Act
        var result = await sut.GetCustomersWithPagination(specification, currentPage: 1, pageSize: 2);

        // Assert
        var countOfRows = result.Count();
        var lastRow = result.Skip(countOfRows - 1).First();
        var firstRow = result.First();
        var sorted = string.Compare(firstRow.LastName, lastRow.LastName, StringComparison.Ordinal);

        Assert.NotNull(result);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(-1, sorted);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomersWithPagination"/> method.
    /// It verifies that a sorted descending list of customers is returned.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomersWithPagination")]
    public async Task GetCustomersWithPagination_ReturnsDescendingSortedList()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        // Create and seed test data
        dbContext.Product.AddRange(GetProductSamples());
        dbContext.Customer.AddRange(GetCustomerSamples());
        await dbContext.SaveChangesAsync();

        var sut = new CustomerRepository(dbContext);
        var specification = CreateSpecification(false);

        // Act
        var result = await sut.GetCustomersWithPagination(specification, currentPage: 1, pageSize: 2);

        // Assert
        var countOfRows = result.Count();
        var lastRow = result.Skip(countOfRows - 1).First();
        var firstRow = result.First();
        var sorted = string.Compare(firstRow.LastName, lastRow.LastName, StringComparison.Ordinal);

        Assert.NotNull(result);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(1, sorted);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomersWithPagination"/> method.
    /// It verifies that a paginated list of customers with filtered search text is returned.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomersWithPagination")]
    public async Task GetCustomersWithPagination_ReturnsPaginatedListWithFilteredSearchText()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        // Create and seed test data
        dbContext.Product.AddRange(GetProductSamples());
        dbContext.Customer.AddRange(GetCustomerSamples());
        await dbContext.SaveChangesAsync();

        var sut = new CustomerRepository(dbContext);
        var specification = CreateSpecification(true, "Customer 1");

        // Act
        var result = await sut.GetCustomersWithPagination(specification, currentPage: 1, pageSize: 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(1, result.TotalPages);
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomersWithPagination"/> method.
    /// It verifies that all customers are returned when pagination is not applied.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomersWithPagination")]
    public async Task GetCustomersWithPagination_ReturnsAllCustomers()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        // Create and seed test data
        dbContext.Product.AddRange(GetProductSamples());
        dbContext.Customer.AddRange(GetCustomerSamples());
        await dbContext.SaveChangesAsync();

        var sut = new CustomerRepository(dbContext);
        var specification = CreateSpecification(true);

        // Act
        var result = await sut.GetCustomersWithPagination(specification, currentPage: 1, pageSize: 5);

        // Assert
        Assert.Equal(5, result.Count); // Ensure 5 customers are returned due to pagination

        var firstCustomer = result.FirstOrDefault(customer => customer.FirstName == "Customer" && customer.LastName == "1");
        Assert.NotNull(firstCustomer);
        Assert.Equal("customer1@example.com", firstCustomer.Email);
        Assert.Equal("1234567890", firstCustomer.PhoneNumber);
        Assert.Equal(2, firstCustomer.Sales.Count);
        Assert.Contains(firstCustomer.Sales, sale => sale.Product.Name == "Product 1");
        Assert.Contains(firstCustomer.Sales, sale => sale.Product.Name == "Product 2");

        var secondCustomer = result.FirstOrDefault(customer => customer.FirstName == "Customer" && customer.LastName == "2");
        Assert.NotNull(secondCustomer);
        Assert.Equal("customer2@example.com", secondCustomer.Email);
        Assert.Equal("9876543210", secondCustomer.PhoneNumber);
        Assert.Single(secondCustomer.Sales);
        Assert.Contains(secondCustomer.Sales, sale => sale.Product.Name == "Product 1");
    }

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.GetCustomersWithPagination"/> method.
    /// It verifies that an empty list is returned when no customers exist in the database.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "GetCustomersWithPagination")]
    public async Task GetCustomersWithPagination_ReturnsEmptyWhenNoCustomerInDb()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        var sut = new CustomerRepository(dbContext);
        var specification = CreateSpecification(true);

        // Act
        var result = await sut.GetCustomersWithPagination(specification, currentPage: 0, pageSize: 5);

        // Assert
        Assert.Empty(result);
    }



    //Update

    /// <summary>
    /// Unit test for the <see cref="CustomerRepository.Update"/> method.
    /// It verifies that no exception is thrown when null models are provided.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "Update")]
    public async Task Update_NullModels_DoesNotThrowException()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        var sut = new CustomerRepository(dbContext);

        // Act and Assert
        try
        {
            sut.Update(null, null);
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
    /// Unit test for the <see cref="CustomerRepository.Update"/> method.
    /// It verifies that no exception is thrown when a bad update model is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "Update")]
    public async Task Update_BadUpdateModel_DoesNotThrowException()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        var existingCustomer = new Customer
        {
            CustomerId = 0,
            FirstName = "Jane",
            LastName = "Watson",
            Email = "jane@example.com",
            PhoneNumber = "1234567890",
            Address = "address 1"
        };
        var customerToUpdate = new Customer
        {
            CustomerId = 1,
            FirstName = "Phil",
            LastName = "Boyce",
            Email = "customer1@example.com",
            PhoneNumber = "1234567890",
            Address = "address 1"
        };

        var sut = new CustomerRepository(dbContext);

        // Act and Assert
        try
        {
            sut.Update(customerToUpdate, existingCustomer);
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
    /// Unit test for the <see cref="CustomerRepository.Update"/> method.
    /// It verifies that no exception is thrown when a bad existing model is provided.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "Update")]
    public async Task Update_BadExistingModel_DoesNotThrowException()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        var existingCustomer = new Customer
        {
            CustomerId = 1,
            FirstName = "Jane",
            LastName = "Watson",
            Email = "jane@example.com",
            PhoneNumber = "1234567890",
            Address = "address 1"
        };
        var customerToUpdate = new Customer
        {
            CustomerId = 0,
            FirstName = "Phil",
            LastName = "Boyce",
            Email = "customer1@example.com",
            PhoneNumber = "1234567890",
            Address = "address 1"
        };

        var sut = new CustomerRepository(dbContext);

        // Act and Assert
        try
        {
            sut.Update(customerToUpdate, existingCustomer);
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
    /// Unit test for the <see cref="CustomerRepository.Update"/> method.
    /// It verifies that an existing customer is successfully updated.
    /// </summary>
    [Fact]
    [Trait("CustomerRepository", "Update")]
    public async Task Update_ExistingCustomer_SuccessfullyUpdatesCustomer()
    {
        // Arrange
        using var factory = new CrudDbContextFactory();
        await using var dbContext = factory.CreateContext();

        var existingCustomer = new Customer
        {
            FirstName = "Jane",
            LastName = "Watson",
            Email = "jane@example.com",
            PhoneNumber = "1234567890",
            Address = "address 1"
        };
        var customerToUpdate = new Customer
        {
            CustomerId = 1,
            FirstName = "Phil",
            LastName = "Boyce",
            Email = "customer1@example.com",
            PhoneNumber = "1234567890",
            Address = "address 1"
        };

        // Create and seed test data
        dbContext.Customer.AddRange(existingCustomer);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        var sut = new CustomerRepository(dbContext);

        // Act
        var customer = await sut.GetCustomer(customerToUpdate.CustomerId);
        sut.Update(customerToUpdate, customer);
        await dbContext.SaveChangesAsync();

        // Assert
        Assert.NotEqual(customerToUpdate.FirstName, existingCustomer.FirstName);
        Assert.NotEqual(customerToUpdate.LastName, existingCustomer.LastName);
        Assert.Equal(customerToUpdate.FirstName, customer.FirstName);
        Assert.Equal(customerToUpdate.LastName, customer.LastName);
        Assert.Equal(customerToUpdate.Email, customer.Email);
        Assert.Equal(customerToUpdate.CustomerId, customer.CustomerId);
    }



    private Specification<Customer> CreateSpecification(bool isAscending, string customerName = "")
    {
        var parameter = new CustomerResourceParameter
        {
            CurrentPage = 1,
            PageSize = 2,
            IsAscending = isAscending,
            SearchText = customerName
        };

        var specification = Specification<Customer>.All;
        specification = specification.And(new CustomerByNameSpecification(parameter.SearchText));

        switch (parameter.SortBy)
        {
            case "Name":
                if (parameter.IsAscending)
                {
                    specification = specification.SortAscending(new SortCustomerByNameSpecification());
                    break;
                }
                specification = specification.SortDescending(new SortCustomerByNameSpecification());
                break;
            default:
                specification = specification.SortAscending(new SortCustomerByNameSpecification());
                break;
        }

        return specification;
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
