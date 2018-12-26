using Common.Api.Validators;
using FluentValidation;
using ToDo.Api.Models;

namespace ToDo.Api.Validators
{
	// Put validator allows you to conditionally set any of the 3 properties
	public class ToDoTaskPutValidator : ValidatorBase<ToDoTaskPut>
	{
		public ToDoTaskPutValidator() =>
			// In order to update the task you must specify one of the properties
			RuleFor(t => t)
				.Must(t =>
					!string.IsNullOrEmpty(t.Description) ||
					!string.IsNullOrEmpty(t.DueDate) ||
					!string.IsNullOrEmpty(t.Completed))
				.WithMessage(ValidationLibrary.AtLeastOneRequired)
				.DependentRules(() =>
				{
					// There is no need to fire the subsequent rules if the top level rule fails

					// Only fire when dueDate is specified
					RuleFor(t => t.DueDate)
						.Must(d => d.IsValidDate())
						.When(t => t.DueDate != null)
						.WithMessage(ValidationLibrary.ValidDateError)
						.Must(d => d.IsFutureDate())
						.When(t => t.DueDate != null)
						.WithMessage(ValidationLibrary.FutureDateError);

					// Only fire when completed is specified
					RuleFor(t => t.Completed)
						.Must(c => c.IsBoolean())
						.When(t => t.Completed != null)
						.WithMessage(ValidationLibrary.BooleanError);
				});
	}
}
