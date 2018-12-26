
namespace ToDo.Api.Models
{
	// When updating a ToDo item you can mark it completed, update the description, and/or due date
	public class ToDoTaskPut : ToDoTaskPost
	{
		public string Completed { get; set; }
	}
}
