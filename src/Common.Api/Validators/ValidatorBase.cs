using FluentValidation;

namespace Common.Api.Validators
{
	public abstract class ValidatorBase<T> : AbstractValidator<T>
	{
		// Usually speaking multiple error message per property are confusing
		// This changes the default behavior on the validators to only produce a single error
		// You can still change it back to CascadeMode.Continue if you want
		protected ValidatorBase() =>
			CascadeMode = CascadeMode.StopOnFirstFailure;
	}
}
