using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Commands.DeleteTask;
using Application.Tasks.Commands.UpdateTask;
using Application.Tasks.Queries.GetTasks;
using Application.Tasks.Queries.GetTasksQuery;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class TaskController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<TasksVm>> Get()
        {
           return await Mediator.Send(new GetTasksQuery() {Name = "test"});
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateTaskCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTaskCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTaskCommand {Id = id});

            return NoContent();
        }
    }
}
