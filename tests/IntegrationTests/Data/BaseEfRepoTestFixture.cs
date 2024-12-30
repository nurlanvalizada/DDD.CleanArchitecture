using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture
{
    protected readonly ApplicationDbContext DbContext;

    protected BaseEfRepoTestFixture()
    {
        var options = CreateNewContextOptions();
        DbContext = new ApplicationDbContext(options, null, new TestCurrentUserService());
    }

    private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        // Create a fresh service provider, and therefore a fresh
        // InMemory database instance.
        var serviceProvider = new ServiceCollection()
                              .AddEntityFrameworkInMemoryDatabase()
                              .BuildServiceProvider();

        // Create a new options instance telling the context to use an
        // InMemory database and the new service provider.
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseInMemoryDatabase("CA").UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }
}