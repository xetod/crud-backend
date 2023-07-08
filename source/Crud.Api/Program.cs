namespace Crud.Api;

/// <summary>
/// The main entry point of the application.
/// </summary>
public class Program
{
    /// <summary>
    /// The main method that is executed when the application starts.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the application.</param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Creates a default host builder and configures the web host using the specified startup class.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the application.</param>
    /// <returns>An instance of <see cref="IHostBuilder"/> used to configure and build the host.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
