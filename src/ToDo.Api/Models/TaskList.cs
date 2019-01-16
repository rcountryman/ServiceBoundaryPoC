using System.Collections.Generic;
using System.Linq;
using Common.Domain;

namespace ToDo.Api.Models
{
	// The root entity we're storing in Mongo
	public class TaskList : AuditEntityBase<IEnumerable<ToDoTask>>
	{
		// Initialize the list to an empty array
		public TaskList() =>
			Entity = Enumerable.Empty<ToDoTask>();
	}
}
