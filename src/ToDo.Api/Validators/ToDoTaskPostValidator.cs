using Common.Api.Validators;
using FluentValidation;
using ToDo.Api.Models;

namespace ToDo.Api.Validators
{
	public class ToDoTaskPostValidator : ValidatorBase<ToDoTaskPost>
	{
		public ToDoTaskPostValidator()
		{
			// On a POST description is required
			RuleFor(t => t.Description)
				.NotEmpty()
				.WithMessage(ValidationLibrary.RequiredError)
				.Must(d => !string.IsNullOrWhiteSpace(d))
				.WithMessage(ValidationLibrary.RequiredError);

			// On a POST due date is required
			RuleFor(t => t.DueDate)
				.NotEmpty()
				.WithMessage(ValidationLibrary.RequiredError)
				.Must(d => d.IsValidDate())
				.WithMessage(ValidationLibrary.ValidDateError)
				.Must(d => d.IsFutureDate())
				.WithMessage(ValidationLibrary.FutureDateError);
		}
	}
}
