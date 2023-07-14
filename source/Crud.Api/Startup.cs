using Crud.Api.Core.AutoMapper;
using Crud.Api.Core.ConfigureDbContexts;
using Crud.Api.Core.DependencyInjections;
using Crud.Api.Core.DependencyInjections.Customers;
using Crud.Api.Core.DependencyInjections.Products;
using Crud.Api.Core.Middlewares;
using Crud.Api.Core.Seed;
using Microsoft.OpenApi.Models;

namespace Crud.Api;

/// <summary>
/// The startup class that configures the application and its services.
/// </summary>
public class Startup
{
    /// <summary>
    /// Gets the configuration of the application.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Gets the current hosting environment.
    /// </summary>
    private IWebHostEnvironment CurrentEnvironment { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration">The configuration of the application.</param>
    /// <param name="webHostEnvironment">The current hosting environment.</param>
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        CurrentEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// Configures the services used by the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions();

        services.AddHttpContextAccessor();

        services.AddControllers();

        services.ConfigureCrudDbContext(Configuration.GetConnectionString("Default"));

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Crud End-Points",
                Version = "v1"
            });
        });

        services.AddCors(p => p.AddPolicy("cors", builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }));

        services.AddAutoMapperConfiguration()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddUnitOfWorkServices()
            .AddProductServices()
            .AddCustomerServices();
    }

    /// <summary>
    /// Configures the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <param name="env">The <see cref="IWebHostEnvironment"/> instance.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Crud v1");
        });

        app.SeedDataBase();

        app.UseRouting();

        app.UseAuthorization();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("cors");
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }
}
