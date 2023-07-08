using Crud.Data.DbContexts;
using Crud.Domain.Entities;

namespace Crud.Api.Core.Seed;

/// <summary>
/// Provides extension methods for seeding data into the database.
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Seeds the database with sample data.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    public static void SeedDataBase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<CrudDbContext>();

        context?.Database.EnsureDeleted();
        context?.Database.EnsureCreated();
        context?.AddData();
    }

    /// <summary>
    /// Adds sample data to the specified <see cref="CrudDbContext"/>.
    /// </summary>
    /// <param name="crudDbContext">The <see cref="CrudDbContext"/> instance.</param>
    /// <returns>The modified <see cref="CrudDbContext"/>.</returns>
    public static CrudDbContext AddData(this CrudDbContext crudDbContext)
    {
        var customers = new List<Customer>();

        // Generate sample customers
        for (var id = 1; id <= 20; id++)
        {
            var customer = new Customer
            {
                FirstName = Faker.Name.FirstName(),
                LastName = Faker.Name.LastName(),
                Address = Faker.Address.SecondaryAddress(),
                Email = Faker.User.Email(),
                PhoneNumber = Faker.Phone.GetPhoneNumber()
            };

            customers.Add(customer);
        }

        var products = new List<Product>();
        // Generate sample products
        for (var id = 1; id <= 20; id++)
        {
            var product = new Product
            {
                Name = Faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(Faker.Commerce.Price())
            };

            products.Add(product);
        }

        var sales = new List<Sale>();
        // Generate sample sales
        for (var id = 1; id <= 20; id++)
        {
            var sale = new Sale
            {
                ProductId = new Random().Next(1, 20),
                CustomerId = id,
                Date = new DateTime(),
                Quantity = new Random().Next(1, 3),
                UnitPrice = Convert.ToDecimal(Faker.Commerce.Price())
            };

            sales.Add(sale);
        }

        // Add generated data to the context and save changes
        crudDbContext.Customer.AddRange(customers);
        crudDbContext.Product.AddRange(products);
        crudDbContext.Sale.AddRange(sales);
        crudDbContext.SaveChanges();

        return crudDbContext;
    }
}
