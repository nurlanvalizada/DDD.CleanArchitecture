using System;
using AppDomain.Common.Entities;
using AppDomain.Enums;
using AppDomain.Events;

namespace AppDomain.Entities;

public class ToDoTask : BaseEntity<Guid>, ICreationAudited, IModificationAudited, IDeletionAudited, ISoftDelete
{
    private ToDoTask()
    {
    }
    
    private ToDoTask(Guid id, string name, TaskPriority priority, TaskState state, Person assignedPerson)
    {
        Id = id;
        Name = name;
        Priority = priority;
        State = state;
        AssignedPerson = assignedPerson;
        AssignedPersonId = assignedPerson.Id;
        CreatedDate = DateTime.Now;
    }
    
    public string Name { get; private set; }

    public bool IsCompleted { get;  private set; }

    public TaskPriority Priority { get; private set; } = TaskPriority.Medium;

    public TaskState State { get; private set; } = TaskState.Active;


    public DateTime CreatedDate { get; set; }

    public long? CreatedUserId { get; set; }


    public DateTime? LastModifiedDate { get; set; }

    public long? LastModifiedUserId { get; set; }


    public bool IsDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public long? DeletedUserId { get; set; }

    public void MarkComplete()
    {
        IsCompleted = true;
        AddDomainEvent(new TaskCompletedEvent(this));
    }

    public void MarkUnComplete()
    {
        IsCompleted = false;
    }

    //navigation properties
    public Guid AssignedPersonId { get; set; }
    public Person AssignedPerson { get; set; }
    
    public static ToDoTask Create(Guid id, string name, TaskPriority priority, TaskState state, Person assignedPerson)
    {
        return new ToDoTask(id, name, priority, state, assignedPerson);
    }
}