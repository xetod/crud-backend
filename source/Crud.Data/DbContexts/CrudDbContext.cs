using Crud.Data.Configurations;
using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data.DbContexts;

/// <summary>
/// Represents the database context for the CRUD application.
/// </summary>
public class CrudDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CrudDbContext"/> class with the provided options.
    /// </summary>
    /// <param name="options">The options for configuring the database context.</param>
    public CrudDbContext(DbContextOptions<CrudDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the DbSet for the Customers table.
    /// </summary>
    public DbSet<Customer> Customer { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for the Products table.
    /// </summary>
    public DbSet<Product> Product { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for the Sales table.
    /// </summary>
    public DbSet<Sale> Sale { get; set; }

    /// <summary>
    /// Configures the database context using the specified model builder.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the database context using the modelBuilder extension method
        modelBuilder.ConfigureCrudDbContext();
    }
}
