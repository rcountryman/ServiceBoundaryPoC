using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Controllers;
using Common.Api.Mappings;
using Common.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Api.Models;

namespace ToDo.Api.Controllers
{
	[Route("api/todo/v1/tasks")]
	public class TasksController : ApiControllerBase
	{
		// Only allow queries from the controller
		// For all commands (writes/side effects) send a command to the worker
		private readonly IMapper _mapper;
		private readonly IQueryRepository<TaskList> _taskListRepository;

		public TasksController(IMapper mapper, IQueryRepository<TaskList> taskListRepository)
		{
			_mapper = mapper;
			_taskListRepository = taskListRepository;
		}

		[HttpDelete("{id:guid}"),
		 ProducesResponseType(StatusCodes.Status204NoContent),
		 ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
		{
			// Read from the database and return notfound if they gave us the wrong ID
			if (await _taskListRepository.CountAsync(tl => tl.Id == UserId && tl.Entity.Any(t => t.Id == id)) > 0)
				return NotFound();
			// TODO: _messageSession.Send();
			return NoContent();
		}

		[HttpGet,
		 ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<ToDoTask>>> GetAsync()
		{
			var list = await _taskListRepository
				.FirstOrDefaultAsync(tl => tl.Id == UserId) ?? new TaskList();
			return Ok(list.Entity);
		}

		[HttpGet("{id:guid}", Name = "GetTaskById"),
		 ProducesResponseType(StatusCodes.Status200OK),
		 ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ToDoTask>> GetAsync([FromRoute] Guid id)
		{
			// Read from the database and return notfound if they gave us the wrong ID
			var list = await _taskListRepository
				.FirstOrDefaultAsync(tl => tl.Id == Guid.Empty && tl.Entity.Any(t => t.Id == id));
			if (list == null)
				return NotFound();
			return Ok(list.Entity.Single(t => t.Id == id));
		}

		[HttpPost,
		 ProducesResponseType(StatusCodes.Status201Created),
		 ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ToDoTask>> PostAsync([FromBody] ToDoTaskPost request)
		{
			await Task.Delay(0);
			var entity = _mapper.Map<ToDoTask>(request);
			// TODO: _messageSession.Send();
			return CreatedAtAction("GetTaskById", new { entity.Id }, entity);
		}

		[HttpPut("{id:guid}"),
		 ProducesResponseType(StatusCodes.Status200OK),
		 ProducesResponseType(StatusCodes.Status400BadRequest),
		 ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ToDoTask>> PutAsync([FromRoute] Guid id, [FromBody] ToDoTaskPut request)
		{
			// Read from the database and return notfound if they gave us the wrong ID
			var list = await _taskListRepository.FirstOrDefaultAsync(tl => tl.Id == Guid.Empty && tl.Entity.Any(t => t.Id == id));
			if (list == null)
				return NotFound();
			var data = list.Entity.Single(t => t.Id == id);
			// If we've gotten here then we have a valid object map to strongly typed version
			var entity = _mapper.Map<ToDoTask>(request, id);
			// TODO: _messageSession.Send();

			// Return back composite graph of what's in the DB & what was sent in
			return Ok(new ToDoTask
			{
				Id = id,
				Completed = entity.Completed.HasValue ?
					entity.Completed.Value ?
						true :
						default(bool?) :
					data.Completed,
				Description = entity.Description ?? data.Description,
				DueDate = entity.DueDate ?? data.DueDate
			});
		}
	}
}
