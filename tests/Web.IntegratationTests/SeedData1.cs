using System.Threading.Tasks;
using AppDomain.Entities;
using AppDomain.ValueObjects;
using Infrastructure.Persistence;

namespace IntegrationTests;

public class SeedData1
{
    public static async Task PopulateTestData(ApplicationDbContext dbContext)
    {
        dbContext.Persons.Add(new Person
        {
            Id = 1,
            Name = "Nurlan",
            Surname = "Valizada",
            Age = 32,
            Address = new Address("street", "city", "state", "country", "zipcode")
        });

        dbContext.Persons.Add(new Person
        {
            Id = 2,
            Name = "Namiq",
            Surname = "Mammadov",
            Age = 25,
            Address = new Address("street", "city", "state", "country", "zipcode")
        });

        dbContext.ToDoTasks.Add(new ToDoTask
        {
            Id = 1,
            Name = "Test1",
            AssignedPersonId = 1,
        });


        dbContext.ToDoTasks.Add(new ToDoTask
        {
            Id = 2,
            Name = "Test2",
            AssignedPersonId = 2,
        });
        
        await dbContext.SaveChangesAsync();
    }
}