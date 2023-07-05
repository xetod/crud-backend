using Crud.Data.Repositories.Customers;
using Crud.Domain.Entities;
using Crud.Test.Helpers;

namespace Crud.Test.Repositories.Customers
{
    public class CustomerRepositoryTests
    {
        [Fact]
        public async Task GetCustomers_ReturnsAllCustomersWithSalesAndProducts()
        {
            // Arrange
            using var factory = new CrudDbContextFactory();
            await using var dbContext = factory.CreateContext();

            // Create and seed test data
            var product1 = new Product { Name = "Product 1" };
            var product2 = new Product { Name = "Product 2" };

            var customer1 = new Customer
            {
                Name = "Customer 1",
                Email = "customer1@example.com",
                PhoneNumber = "1234567890",
                Address = "address 1",
                Sales = new List<Sale>
                {
                    new Sale { Product = product1 },
                    new Sale { Product = product2 }
                }
            };

            var customer2 = new Customer
            {
                Name = "Customer 2",
                Email = "customer2@example.com",
                PhoneNumber = "9876543210",
                Address = "address 2",
                Sales = new List<Sale>
                {
                    new Sale { Product = product1 }
                }
            };

            dbContext.Product.AddRange(product1, product2);
            dbContext.Customer.AddRange(customer1, customer2);
            await dbContext.SaveChangesAsync();

            var sut = new CustomerRepository(dbContext);

            // Act
            var result = await sut.GetCustomers();

            // Assert
            // Ensure all customers are retrieved
            Assert.Equal(2, result.Count);

            // Check customer 1 and its sales
            var firstCustomer = result.FirstOrDefault(customer => customer.Name == "Customer 1");
            Assert.NotNull(firstCustomer);
            Assert.Equal("customer1@example.com", firstCustomer.Email);
            Assert.Equal("1234567890", firstCustomer.PhoneNumber);
            Assert.Equal(2, firstCustomer.Sales.Count);
            Assert.Contains(firstCustomer.Sales, sale => sale.Product.Name == "Product 1");
            Assert.Contains(firstCustomer.Sales, sale => sale.Product.Name == "Product 2");

            // Check customer 2 and its sales
            var secondCustomer = result.FirstOrDefault(customer => customer.Name == "Customer 2");
            Assert.NotNull(secondCustomer);
            Assert.Equal("customer2@example.com", secondCustomer.Email);
            Assert.Equal("9876543210", secondCustomer.PhoneNumber);
            Assert.Single(secondCustomer.Sales);
            Assert.Contains(secondCustomer.Sales, sale => sale.Product.Name == "Product 1");
        }

        [Fact]
        public async Task GetCustomers_ReturnsAllCustomersWithNoSales()
        {
            // Arrange
            using var factory = new CrudDbContextFactory();
            await using var dbContext = factory.CreateContext();

            // Create and seed test data
            var customer1 = new Customer
            {
                Name = "Customer 1",
                Email = "customer1@example.com",
                PhoneNumber = "1234567890",
                Address = "address 1"
            };

            var customer2 = new Customer
            {
                Name = "Customer 2",
                Email = "customer2@example.com",
                PhoneNumber = "9876543210",
                Address = "address 2"
            };

            dbContext.Customer.AddRange(customer1, customer2);
            await dbContext.SaveChangesAsync();

            var sut = new CustomerRepository(dbContext);

            // Act
            var result = await sut.GetCustomers();
            // Ensure all customers are retrieved
            Assert.Equal(2, result.Count);

            // Check customer 1 
            var firstCustomer = result.FirstOrDefault(customer => customer.Name == "Customer 1");
            Assert.NotNull(firstCustomer);
            Assert.Equal("customer1@example.com", firstCustomer.Email);
            Assert.Equal("1234567890", firstCustomer.PhoneNumber);
            Assert.Empty(firstCustomer.Sales);

            // Check customer 2
            var secondCustomer = result.FirstOrDefault(customer => customer.Name == "Customer 2");
            Assert.NotNull(secondCustomer);
            Assert.Equal("customer2@example.com", secondCustomer.Email);
            Assert.Equal("9876543210", secondCustomer.PhoneNumber);
            Assert.Empty(secondCustomer.Sales);
        }

        [Fact]
        public async Task GetCustomers_ReturnsEmptyWhenNoCustomerInDb()
        {
            // Arrange
            using var factory = new CrudDbContextFactory();
            await using var dbContext = factory.CreateContext();

            var sut = new CustomerRepository(dbContext);

            // Act
            var result = await sut.GetCustomers();

            // Assert
            Assert.Empty(result);
        }
    }
}