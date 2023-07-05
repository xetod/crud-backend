using Crud.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crud.Test.Helpers;

public class CrudDbContextFactory : IDisposable
{
    private CrudDbContext _context;

    public CrudDbContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CrudDbContext>();

        optionsBuilder.UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=CrudIntegrationTests");

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();

        var options = optionsBuilder.Options;

        _context = new CrudDbContext(options);

        _context.Database.EnsureDeleted();

        _context.Database.EnsureCreated();

        return _context;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}