using Crud.Api.Core.ConfigureDbContexts;
using Crud.Data.DbContexts;

namespace Crud.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    private IWebHostEnvironment CurrentEnvironment { get; set; }

    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        CurrentEnvironment = webHostEnvironment;
    }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions();
        
        services.AddHttpContextAccessor();

        services.AddControllers();

        services.ConfigureCrudDbContext(Configuration.GetConnectionString("Default"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CrudDbContext crudDbContext)
    {
        crudDbContext.Database.EnsureDeleted();
        crudDbContext.Database.EnsureCreated();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}



