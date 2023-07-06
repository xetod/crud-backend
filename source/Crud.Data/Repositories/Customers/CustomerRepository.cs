using System.Drawing;
using Crud.Data.Core.PagedLists;
using Crud.Data.DbContexts;
using Crud.Data.Repositories.Core.Repositories;
using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryService.Data.Core.Specifications;

namespace Crud.Data.Repositories.Customers;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(CrudDbContext context) : base(context) { }

    public async Task<PagedList<Customer>> GetCustomersWithPagination(Specification<Customer> specification, int currentPage = 0, int pageSize = 0)
    {
        var query = CrudDbContext
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
            .AsQueryable();
        
        query = SpecificationEvaluator<Customer>.GetQuery(query, specification);

        var customers = await PagedList<Customer>.CreateAsync(query, currentPage, pageSize);

        return customers;
    }

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