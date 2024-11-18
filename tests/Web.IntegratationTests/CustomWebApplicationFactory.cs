using System;
using System.Linq;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            services.Remove(dbContextDescriptor);

            // Add a database context (AppDbContext) using an in-memory database for testing.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("testdb");
            });

            // Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database contexts
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var appDb = scopedServices.GetRequiredService<ApplicationDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            // Ensure the database is created.
            appDb.Database.EnsureCreated();

            try
            {
                // Seed the database with some specific test data.
                SeedData.PopulateTestData(appDb).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the database with test messages");
            }
        });
    }
}