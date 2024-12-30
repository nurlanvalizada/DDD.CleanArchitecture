using System;
using System.IO;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class ApplicationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(GetDesignTimeConnectionString());

        return new ApplicationDbContext(optionsBuilder.Options, null, new TestCurrentUserService());
    }
    
    private static string GetDesignTimeConnectionString()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "Web");
        var configuration = new ConfigurationBuilder()
                            .SetBasePath(path)
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile("appsettings.Development.json")
                            .Build();
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("Connection string not found at: " + path);
        }
        
        return connectionString;
    }
}