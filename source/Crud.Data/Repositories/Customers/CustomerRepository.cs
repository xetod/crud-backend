using Crud.Data.DbContexts;
using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Repositories.Customers;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(CrudDbContext context) : base(context) { }

    public async Task<List<Customer>> GetCustomers()
    {
        return await CrudDbContext
            .Customer
            .Include(customer => customer.Sales)
            .ThenInclude(sale => sale.Product)
            .Select(customer => new Customer
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Sales = customer.Sales.Select(sale => new Sale
                {
                    Product = new Product
                    {
                        Name = sale.Product.Name
                    }
                })
                .ToList()
            })
            .ToListAsync();
    }


    private CrudDbContext CrudDbContext => Context as CrudDbContext;
}