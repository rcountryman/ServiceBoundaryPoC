using System;
using NodaTime;

namespace ToDo.Api.Models
{
	public class ToDoTask : ToDoTaskPut
	{
		// Add an id to access the tasks from the array
		public Guid Id { get; set; }

		// Description remains a string so no need to hide the property with a strongly typed replacement

		// DueDate is a date only value so hide string implementation
		// Also make it nullable to not blow up map on PUT
		public new LocalDate? DueDate { get; set; }

		// Completed is an optional boolean only set to true when completed
		public new bool? Completed { get; set; }
	}
}
