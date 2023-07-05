using Crud.Data.DbContexts;
using Crud.Domain.Entities;

namespace Crud.Api.Core.Seed;

/// <summary>
/// Only for showcase purposes no such thing in real applications
/// </summary>
public static class SeedData
{
    public static void SeedDataBase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<CrudDbContext>();

        context?.Database.EnsureDeleted();

        context?.Database.EnsureCreated();

        context?.AddData();
    }

    public static CrudDbContext AddData(this CrudDbContext crudDbContext)
    {
        var customers = new List<Customer>();

        for (var id = 1; id <= 20; id++)
        {
            var customer = new Customer
            {
                Name = Faker.Name.FullName(),
                Address = Faker.Address.SecondaryAddress(),
                Email = Faker.User.Email(),
                PhoneNumber = Faker.Phone.GetPhoneNumber()
            };

            customers.Add(customer);
        }

        var products = new List<Product>();
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
        for (var id = 1; id <= 20; id++)
        {
            var sale = new Sale
            {
                ProductId = new Random().Next(1, 20),
                CustomerId = new Random().Next(1, 20),
                Date = new DateTime(),
                Quantity = new Random().Next(1, 3),
                UnitPrice = Convert.ToDecimal(Faker.Commerce.Price())
            };

            sales.Add(sale);
        }

        crudDbContext.Customer.AddRange(customers);
        crudDbContext.Product.AddRange(products);
        crudDbContext.Sale.AddRange(sales);
        crudDbContext.SaveChanges();

        return crudDbContext;
    }
}