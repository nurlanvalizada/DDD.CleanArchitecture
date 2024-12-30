using AppDomain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Data;

public class RepositoryIntegrationTests : BaseEfRepoTestFixture, IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await SeedData.PopulateTestDataAsync(DbContext);
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }
   
    [Fact]
    public async Task GetAllListIncluding_ShouldReturnEntitiesWithIncludedProperties()
    {
        var repository = new EfRepository<Person, Guid>(DbContext);

        var entities = await repository.GetAllListIncludingAsync(CancellationToken.None, p => p.Tasks);

        Assert.NotNull(entities);
        var firstPerson = entities.FirstOrDefault(p => p.Id == SeedData.PersonId1);
        Assert.NotNull(firstPerson);
        Assert.NotNull(firstPerson.Tasks);
        Assert.Single(firstPerson.Tasks);
        Assert.Equal("Task1", firstPerson.Tasks.First().Name);
        Assert.Equal(2, entities.Count());
    }

    [Fact]
    public async Task GetAllListIncluding_ShouldReturnEntitiesWithOutProperties()
    {
        var repository = new EfRepository<Person, Guid>(DbContext);
        var entities = await repository.GetAllListIncludingAsync(CancellationToken.None);

        Assert.NotNull(entities);
        Assert.Equal(2, entities.Count);

        var firstPerson = entities.FirstOrDefault(p => p.Id == SeedData.PersonId1);
        Assert.NotNull(firstPerson);
        Assert.True(firstPerson.Tasks.Count == 1);

        var secondPerson = entities.FirstOrDefault(p => p.Id == SeedData.PersonId2);
        Assert.NotNull(secondPerson);
        Assert.True(secondPerson.Tasks.Count == 1);
    }

    [Fact]
    public async Task GetFirstIncluding_WithIdInInput_ShouldReturnEntitiesWithIncludedProperties()
    {
        var repository = new EfRepository<Person, Guid>(DbContext);

        var entity = await repository.GetFirstIncludingAsync(SeedData.PersonId1, CancellationToken.None, p => p.Tasks);

        Assert.NotNull(entity);
        Assert.NotNull(entity.Tasks);
        Assert.Single(entity.Tasks);
        Assert.Equal("Task1", entity.Tasks.First().Name);
    }

    [Fact]
    public async Task GetFirstIncluding_WithFuncInInput_ShouldReturnEntitiesWithIncludedProperties()
    {
        var repository = new EfRepository<Person, Guid>(DbContext);

        var entity = await repository.GetFirstIncludingAsync(x => x.Id == SeedData.PersonId1, CancellationToken.None, p => p.Tasks);

        Assert.NotNull(entity);
        Assert.NotNull(entity.Tasks);
        Assert.Single(entity.Tasks);
        Assert.Equal("Task1", entity.Tasks.First().Name);
    }

    [Fact]
    public async Task Update_ShouldModifyEntity()
    {
        var repository = new EfRepository<Person, Guid>(DbContext);
        var person = await repository.GetFirstAsync(x => true, CancellationToken.None);

        person.Update("UpdatedName", person.Surname, person.Age, person.Address);
        await repository.UpdateAsync(person);
        await DbContext.SaveChangesAsync();

        var updatedPerson = await repository.GetFirstAsync(person.Id, CancellationToken.None);

        Assert.NotNull(updatedPerson);
        Assert.Equal("UpdatedName", updatedPerson.Name);
    }

    [Fact]
    public async Task DeleteWhere_ShouldDeleteEntity()
    {
        var repository = new EfRepository<Person, Guid>(DbContext);
        await repository.DeleteWhereAsync(x => x.Id == SeedData.PersonId2);
        await repository.CommitAsync();

        var persons = await repository.GetAllListAsync();

        Assert.Single(persons);
        Assert.DoesNotContain(persons, p => p.Id == SeedData.PersonId2);
    }
}