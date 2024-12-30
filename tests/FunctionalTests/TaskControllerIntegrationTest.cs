using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppDomain.ToDoTasks;
using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Commands.UpdateTask;
using Application.Tasks.Queries.GetTasks;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Web;
using Xunit;

namespace FunctionalTests;

public class TaskControllerIntegrationTest(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task Get_ShouldReturnTask()
    {
        var query = new GetTasksQuery {Name = "Task" };
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

        var jsonContent = JsonConvert.SerializeObject(command);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/Task", stringContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var createdTaskId = JsonConvert.DeserializeObject<Guid>(responseString);

        Assert.True(createdTaskId != Guid.Empty);
    }

   [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        var response = await _client.DeleteAsync($"api/Task/{SeedData.TaskId2}");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var repository = new EfRepository<ToDoTask, Guid>(dbContext);
        var deletedTask = await repository.GetFirstAsync(SeedData.PersonId2);
        Assert.Null(deletedTask);
    }

   [Fact]
    public async Task Update_ShouldModifyEntity()
    {
        var updateCommand = new UpdateTaskCommand
        {
            Id = SeedData.TaskId1,
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