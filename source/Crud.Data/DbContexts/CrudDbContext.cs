using Crud.Data.Configurations;
using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data.DbContexts;

public class CrudDbContext : DbContext
{
    public CrudDbContext(DbContextOptions<CrudDbContext> options) : base(options) { }

    public DbSet<Customer> Customer { get; set; }

    public DbSet<Product> Product { get; set; }

    public DbSet<Sale> Sale { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureCrudDbContext();
    }

}