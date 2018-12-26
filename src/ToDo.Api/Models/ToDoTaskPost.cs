
namespace ToDo.Api.Models
{
	// When adding a ToDo item you can only add the description & dueDate
	public class ToDoTaskPost
	{
		public string Description { get; set; }
		public string DueDate { get; set; }
	}
}
