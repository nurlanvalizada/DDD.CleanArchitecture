using System;
using System.Threading.Tasks;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Web;
using Xunit;

namespace FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            Console.WriteLine("Reconfiguring services...");
            
            services.RemoveAll<ApplicationDbContext>();
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll(typeof(DbContextOptions<>));
            services.RemoveAll(typeof(DbContextOptions));
            services.RemoveAll(typeof(IDbContextOptionsConfiguration<ApplicationDbContext>));
            services.RemoveAll<ServiceProviderAccessor>();

            // Add a database context (AppDbContext) using an in-memory database for testing.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("testdb");
                Console.WriteLine("Added InMemoryDatabase for ApplicationDbContext");
            });

            // Build the service provider.
            var sp = services.BuildServiceProvider();
            
            // Create a scope to obtain a reference to the database contexts
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var appDb = scopedServices.GetRequiredService<ApplicationDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();
            // Ensure the database is created.
            appDb.Database.EnsureCreated();
            
            try
            {
                // Seed the database with some specific test data.
                SeedData.PopulateTestDataAsync(appDb).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the database with test messages");
            }
        });
        
        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync()
    {
        //await SeedData.PopulateTestDataAsync(DbContext);
    }

    public async Task DisposeAsync()
    {
        //await DbContext.DisposeAsync();
    }
}