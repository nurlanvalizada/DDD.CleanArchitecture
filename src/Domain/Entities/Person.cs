using System;
using System.Collections.Generic;
using AppDomain.Common.Entities;
using AppDomain.ToDoTasks;
using AppDomain.ValueObjects;

namespace AppDomain.Entities;

public class Person : AggregateRoot<Guid>
{
    private readonly List<ToDoTask> _tasks = [];
    
    private Person()
    {
    }
    
    private Person(Guid id, string name, string surname, short age, Address address)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Age = age;
        Address = address;
    }
    
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public short Age { get; private set; }
    public Address Address { get; private set; }
    public IReadOnlyCollection<ToDoTask> Tasks => _tasks.AsReadOnly();
    
    public void AssignTask(ToDoTask task)
    {
        _tasks.Add(task);
    }
    
    public void Update(string name, string surname, short age, Address address)
    {
        Name = name;
        Surname = surname;
        Age = age;
        Address = address;
    }
    
    public static Person Create(Guid id, string name, string surname, short age, Address address)
    {
        return new Person(id, name, surname, age, address);
    }
}