using Crud.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crud.Test.Helpers;

/// <summary>
/// Factory class for creating and managing instances of the CrudDbContext.
/// </summary>
public class CrudDbContextFactory : IDisposable
{
    private CrudDbContext _context;

    /// <summary>
    /// Creates a new instance of CrudDbContext with the configured options.
    /// </summary>
    /// <returns>The created CrudDbContext instance.</returns>
    public CrudDbContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CrudDbContext>();

        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CrudIntegrationTests");

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();

        var options = optionsBuilder.Options;

        _context = new CrudDbContext(options);

        _context.Database.EnsureDeleted();
        // Ensures that the database is deleted (if it exists) before creating a new one.

        _context.Database.EnsureCreated();
        // Creates the database and its schema if they do not exist.

        return _context;
    }

    /// <summary>
    /// Disposes the CrudDbContext instance.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
    }
}
