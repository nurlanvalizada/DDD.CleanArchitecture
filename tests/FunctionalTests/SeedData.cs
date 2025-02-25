using System;
using System.Threading.Tasks;
using AppDomain.Entities;
using AppDomain.ToDoTasks;
using AppDomain.ValueObjects;
using Infrastructure.Persistence;

namespace FunctionalTests;

public class SeedData
{
    public static Guid PersonId1 = Guid.NewGuid();
    public static Guid PersonId2 = Guid.NewGuid();
    
    public static Guid TaskId1 = Guid.NewGuid();
    public static Guid TaskId2 = Guid.NewGuid();
    
    public static async Task PopulateTestDataAsync(ApplicationDbContext dbContext)
    {
        var person1 = Person.Create(PersonId1, "Nurlan", "Valizada", 32, new Address("street", "city", "state", "country", "zipcode"));
        var person2 = Person.Create(PersonId2, "Namiq", "Valiyev", 26, new Address("street", "city", "state", "country", "zipcode"));
        
        dbContext.Persons.Add(person1);
        dbContext.Persons.Add(person2);

        dbContext.ToDoTasks.Add(ToDoTask.Create(TaskId1, "Task1", TaskPriority.Medium, TaskState.Active, person1));
        dbContext.ToDoTasks.Add(ToDoTask.Create(TaskId2, "Task2", TaskPriority.Medium, TaskState.Active, person2));
        
        await dbContext.SaveChangesAsync();
    }
}