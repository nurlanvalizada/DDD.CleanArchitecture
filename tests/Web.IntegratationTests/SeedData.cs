using AppDomain.Entities;
using AppDomain.ValueObjects;
using Infrastructure.Persistence;

namespace IntegrationTests;

public class SeedData
{
    public static void PopulateTestData(ApplicationDbContext dbContext)
    {
        dbContext.Persons.Add(new Person
        {
            Id = 1,
            Name = "Mahmud",
            Surname = "Sofiyev",
            Age = 26,
            Address = new Address("street", "city", "state", "country", "zipcode")
        });

        dbContext.Persons.Add(new Person
        {
            Id = 2,
            Name = "Mahmud1",
            Surname = "Sofiyev1",
            Age = 26,
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
        dbContext.SaveChanges();
    }
}