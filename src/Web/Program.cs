using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Infrastructure.Data;
using tm20trial.Infrastructure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi3(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });


app.MapEndpoints();

app.Run();

public partial class Program {
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var configuration = host.Services.GetService<IConfiguration>();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var applySeeds = configuration.GetSection("Technical").GetValue<bool>("SeedDatabase");

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var identityService = services.GetRequiredService<IIdentityService>();
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    var seeds = new List<IApplicationDbSeed>
                    {
                        // new ApplicationRolesSeed(roleManager),
                 
                    };

                    if (applySeeds)
                    {
                        foreach (var seed in seeds)
                        {
                            await seed.PreMigrationSeedAsync();
                        }
                    }

                    if (context.Database.IsSqlServer())
                    {
                        await context.Database.MigrateAsync();
                    }

                    if (applySeeds)
                    {
                        foreach (var seed in seeds)
                        {
                            logger.LogInformation($"Start seed {seed.GetType().Name}");

                            await seed.PostMigrationSeedAsync();

                            logger.LogInformation($"End seed {seed.GetType().Name}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("An error occurred while migrating or seeding the database.", ex);
                    throw;
                }
            }

            await host.RunAsync();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Host terminated unexpectedly");
            Console.WriteLine(e);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((context, configuration) => { }, writeToProviders: true)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

                webBuilder.UseSentry();
            });
}
