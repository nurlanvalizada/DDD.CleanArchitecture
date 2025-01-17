﻿using System;
using AppDomain.ToDoTasks;
using Application.Common.Mappings;

namespace Application.Tasks.Queries.GetTasks;

public record TaskDto : IMapFrom<ToDoTask>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public TaskPriority Priority { get; set; }

    public TaskState State { get; set; }
}