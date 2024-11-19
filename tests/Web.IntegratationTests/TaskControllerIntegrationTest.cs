using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppDomain.Entities;
using AppDomain.Enums;
using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Commands.UpdateTask;
using Application.Tasks.Queries.GetTasks;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests;

public class TaskControllerIntegrationTest(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    public async Task InitializeAsync()
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        await SeedData.PopulateTestDataAsync(context);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Get_ShouldReturnTask()
    {
        var query = new GetTasksQuery {Name = "Test" };
        var queryString = $"?Name={query.Name}";

        var response = await _client.GetAsync($"/api/Task{queryString}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var tasks = JsonConvert.DeserializeObject<TasksVm>(responseString);

        Assert.NotNull(tasks);
        Assert.Equal(2, tasks.Tasks.Count);
    }

    [Fact]
    public async Task Create_ValidCommand_ShouldCreateTaskSuccessfully()
    {
        var command = new CreateTaskCommand
        {
            Name = "New Task",
            State = TaskState.Active,
            Priority = TaskPriority.High,
            AssignedPersonId = SeedData.PersonId1
        };

        var expectedTaskId = 3;

        var jsonContent = JsonConvert.SerializeObject(command);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/Task", stringContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var createdTaskId = JsonConvert.DeserializeObject<int>(responseString);

        Assert.Equal(expectedTaskId, createdTaskId);
    }

    [Fact]
    public async Task Delete_ShouldRetrunNoContent()
    {
        var response = await _client.DeleteAsync($"api/Task/2");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var repository = new EfRepository<ToDoTask, Guid>(dbContext);
        var deletedTask = await repository.GetFirst(SeedData.PersonId2);
        Assert.Null(deletedTask);
    }

    [Fact]
    public async Task Update_ShouldModifyEntity()
    {
        var updateCommand = new UpdateTaskCommand
        {
            Id = SeedData.PersonId1,
            Name = "UpdatedTask"
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(updateCommand), Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/api/Task/{updateCommand.Id}", jsonContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var updatedTask = await context.ToDoTasks.FindAsync(updateCommand.Id);

        Assert.NotNull(updatedTask);
        Assert.Equal("UpdatedTask", updatedTask.Name);
    }

}