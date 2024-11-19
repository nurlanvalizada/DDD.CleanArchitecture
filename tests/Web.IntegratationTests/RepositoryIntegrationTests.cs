using System;
using System.Linq;
using System.Threading.Tasks;
using AppDomain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests;

public class RepositoryIntegrationTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure the database is deleted and then recreated
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // Seed the database with specific test data
        await SeedData.PopulateTestDataAsync(context);
    }

    public Task DisposeAsync()
    {
        // Cleanup logic can go here if necessary
        return Task.CompletedTask;
    }


    [Fact]
    public async Task GetAllListIncluding_ShouldReturnEntitiesWithIncludedProperties()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var repository = new EfRepository<Person, Guid>(context);

        var entities = await repository.GetAllIncluding(p => p.Tasks).ToListAsync();

        Assert.NotNull(entities);
        var firstPerson = entities.FirstOrDefault(p => p.Id == SeedData.PersonId1);
        Assert.NotNull(firstPerson);
        Assert.NotNull(firstPerson.Tasks);
        Assert.Single(firstPerson.Tasks);
        Assert.Equal("Test1", firstPerson.Tasks.First().Name);
        Assert.Equal(2,entities.Count());
    }

    [Fact]
    public async Task GetAllListIncluding_ShouldReturnEntitiesWithOutProperties()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var repository = new EfRepository<Person, Guid>(context);
        var entities = await repository.GetAllIncluding().ToListAsync();

        Assert.NotNull(entities);
        Assert.Equal(2, entities.Count);

        var firstPerson = entities.FirstOrDefault(p => p.Id == SeedData.PersonId1);
        Assert.NotNull(firstPerson);
        Assert.Null(firstPerson.Tasks);

        var secondPerson = entities.FirstOrDefault(p => p.Id == SeedData.PersonId2);
        Assert.NotNull(secondPerson);
        Assert.Null(secondPerson.Tasks);
    }

    [Fact]
    public async Task GetFirstIncluding_WithIdInInput_ShouldReturnEntitiesWithIncludedProperties()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var repository = new EfRepository<Person, Guid>(context);

        var entity = await repository.GetFirstIncluding(SeedData.PersonId1, p => p.Tasks);

        Assert.NotNull(entity);
        Assert.NotNull(entity.Tasks);
        Assert.Single(entity.Tasks);
        Assert.Equal("Test1", entity.Tasks.First().Name);
    }

    [Fact]
    public async Task GetFirstIncluding_WithFuncInInput_ShouldReturnEntitiesWithIncludedProperties()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var repository = new EfRepository<Person, Guid>(context);

        var entity = await repository.GetFirstIncluding(x => x.Id == SeedData.PersonId1, p => p.Tasks);

        Assert.NotNull(entity);
        Assert.NotNull(entity.Tasks);
        Assert.Single(entity.Tasks);
        Assert.Equal("Test1", entity.Tasks.First().Name);
    }

    [Fact]
    public async Task Update_ShouldModifyEntity()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var repository = new EfRepository<Person, Guid>(context);
        var person = await context.Persons.FirstAsync();

        person.Update("UpdatedName", person.Surname, person.Age, person.Address);
        await repository.Update(person);
        await context.SaveChangesAsync();

        var updatedPerson = await context.Persons.FindAsync(person.Id);

        Assert.NotNull(updatedPerson);
        Assert.Equal("UpdatedName", updatedPerson.Name);
    }

    [Fact]
    public async Task DeleteWhere_ShouldDeleteEntity()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var repository = new EfRepository<Person, Guid>(context);
        await repository.DeleteWhere(x => x.Id == SeedData.PersonId2);
        await context.SaveChangesAsync();

        var persons = await repository.GetAllList();

        Assert.Single(persons);
        Assert.DoesNotContain(persons, p => p.Id == SeedData.PersonId2);
    }
}