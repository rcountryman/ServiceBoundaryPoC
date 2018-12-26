using System;
using System.Linq;
using Common.Api.Validators;
using FluentValidation;
using ToDo.Api.Models;
using ToDo.Api.Validators;
using Xunit;

namespace ToDo.Api.Tests
{
	public class ToDoTaskPostValidatorTests
	{
		private readonly IValidator<ToDoTaskPost> _validator =
			new ToDoTaskPostValidator();

		[Fact]
		private void TestPostRequestAllBadValues()
		{
			// Arrange
			var source = new ToDoTaskPost
			{ Description = "       ", DueDate = "hello world" };
			// Act
			var result = _validator.Validate(source);
			// Assert
			Assert.False(result.IsValid);
			Assert.Equal(2, result.Errors.Count);
			Assert.Equal(ValidationLibrary.RequiredError,
				result.Errors.Single(e => e.PropertyName == "Description")
					.ErrorMessage);
			Assert.Equal(ValidationLibrary.ValidDateError,
				result.Errors.Single(e => e.PropertyName == "DueDate")
					.ErrorMessage);
		}

		[Fact]
		private void TestPostRequestDueDates()
		{
			// Arrange
			var badFormatSource = new ToDoTaskPost
			{ Description = "My ToDo for Today", DueDate = "hello world" };
			var badDateSource = new ToDoTaskPost
			{ Description = "My ToDo for Today", DueDate = "2022-02-30" };
			var pastDateSource = new ToDoTaskPost
			{ Description = "My ToDo for Today", DueDate = "2002-01-31" };
			var goodSource = new ToDoTaskPost
			{
				Description = "My ToDo for Today",
				DueDate = DateTime.Today.ToString("yyyy-MM-dd")
			};
			// Act
			var badFormatResult = _validator.Validate(badFormatSource);
			var badDateValueResult = _validator.Validate(badDateSource);
			var pastDateResult = _validator.Validate(pastDateSource);
			var goodResult = _validator.Validate(goodSource);
			// Assert
			Assert.False(badFormatResult.IsValid);
			Assert.False(badDateValueResult.IsValid);
			Assert.False(pastDateResult.IsValid);
			Assert.True(goodResult.IsValid);
			Assert.Equal(1, badFormatResult.Errors.Count);
			Assert.Equal(1, badDateValueResult.Errors.Count);
			Assert.Equal(1, pastDateResult.Errors.Count);
			Assert.False(goodResult.Errors.Any());
			Assert.Equal(ValidationLibrary.ValidDateError,
				badFormatResult.Errors.Single().ErrorMessage);
			Assert.Equal(ValidationLibrary.ValidDateError,
				badDateValueResult.Errors.Single().ErrorMessage);
			Assert.Equal(ValidationLibrary.FutureDateError,
				pastDateResult.Errors.Single().ErrorMessage);
		}

		[Fact]
		private void TestPostRequestEmpty()
		{
			// Arrange
			var source = new ToDoTaskPost();
			// Act
			var result = _validator.Validate(source);
			// Assert
			Assert.False(result.IsValid);
			Assert.Equal(2, result.Errors.Count);
			Assert.Equal(ValidationLibrary.RequiredError,
				result.Errors.Single(p => p.PropertyName == "Description")
					.ErrorMessage);
			Assert.Equal(ValidationLibrary.RequiredError,
				result.Errors.Single(p => p.PropertyName == "DueDate")
					.ErrorMessage);
		}
	}
}
