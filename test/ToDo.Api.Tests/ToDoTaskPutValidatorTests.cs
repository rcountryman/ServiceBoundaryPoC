using System;
using System.Linq;
using Common.Api.Validators;
using FluentValidation;
using ToDo.Api.Models;
using ToDo.Api.Validators;
using Xunit;

namespace ToDo.Api.Tests
{
	public class ToDoTaskPutValidatorTests
	{
		private readonly IValidator<ToDoTaskPut> _validator =
			new ToDoTaskPutValidator();

		[Fact]
		private void TestPutRequestAllValues()
		{
			// Arrange
			var badSource = new ToDoTaskPut
			{ Completed = "hello world", DueDate = "hello world" };
			var goodSource = new ToDoTaskPut
			{
				Completed = "true",
				DueDate = DateTime.Today.ToString("yyyy-MM-dd")
			};
			// Act
			var badResult = _validator.Validate(badSource);
			var goodResult = _validator.Validate(goodSource);

			// Assert
			Assert.False(badResult.IsValid);
			Assert.True(goodResult.IsValid);
			Assert.Equal(2, badResult.Errors.Count);
			Assert.False(goodResult.Errors.Any());
			Assert.Equal(ValidationLibrary.BooleanError,
				badResult.Errors.Single(e => e.PropertyName == "Completed")
					.ErrorMessage);
			Assert.Equal(ValidationLibrary.ValidDateError,
				badResult.Errors.Single(e => e.PropertyName == "DueDate")
					.ErrorMessage);
		}

		[Fact]
		private void TestPutRequestCompleted()
		{
			// Arrange
			var badSource = new ToDoTaskPut { Completed = "hello world" };
			var goodUpperSource = new ToDoTaskPut { Completed = "TRUE" };
			var goodLowerSource = new ToDoTaskPut { Completed = "false" };
			// Act
			var badResult = _validator.Validate(badSource);
			var goodUpperResult = _validator.Validate(goodUpperSource);
			var goodLowerResult = _validator.Validate(goodLowerSource);

			// Assert
			Assert.False(badResult.IsValid);
			Assert.True(goodUpperResult.IsValid);
			Assert.True(goodLowerResult.IsValid);
			Assert.Equal(1, badResult.Errors.Count);
			Assert.False(goodUpperResult.Errors.Any());
			Assert.False(goodLowerResult.Errors.Any());
			Assert.Equal(ValidationLibrary.BooleanError,
				badResult.Errors.Single().ErrorMessage);
		}

		[Fact]
		private void TestPutRequestDueDates()
		{
			// Arrange
			var badFormatSource = new ToDoTaskPut { DueDate = "hello world" };
			var badDateSource = new ToDoTaskPut { DueDate = "2022-02-30" };
			var pastDateSource = new ToDoTaskPut { DueDate = "2002-01-31" };
			var goodDateSource = new ToDoTaskPut
			{ DueDate = DateTime.Today.ToString("yyyy-MM-dd") };
			// Act
			var badFormatResult = _validator.Validate(badFormatSource);
			var badDateValueResult = _validator.Validate(badDateSource);
			var pastDateResult = _validator.Validate(pastDateSource);
			var goodDateResult = _validator.Validate(goodDateSource);

			// Assert
			Assert.False(badFormatResult.IsValid);
			Assert.False(badDateValueResult.IsValid);
			Assert.False(pastDateResult.IsValid);
			Assert.True(goodDateResult.IsValid);
			Assert.Equal(1, badFormatResult.Errors.Count);
			Assert.Equal(1, badDateValueResult.Errors.Count);
			Assert.Equal(1, pastDateResult.Errors.Count);
			Assert.False(goodDateResult.Errors.Any());
			Assert.Equal(ValidationLibrary.ValidDateError,
				badFormatResult.Errors.Single().ErrorMessage);
			Assert.Equal(ValidationLibrary.ValidDateError,
				badDateValueResult.Errors.Single().ErrorMessage);
			Assert.Equal(ValidationLibrary.FutureDateError,
				pastDateResult.Errors.Single().ErrorMessage);
		}


		[Fact]
		private void TestPutRequestEmpty()
		{
			// Arrange
			var source = new ToDoTaskPut();
			// Act
			var result = _validator.Validate(source);

			// Assert
			Assert.False(result.IsValid);
			Assert.Equal(1, result.Errors.Count);
			Assert.Equal(ValidationLibrary.AtLeastOneRequired,
				result.Errors.Single().ErrorMessage);
		}
	}
}
