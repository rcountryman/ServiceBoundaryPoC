using System;
using Common.Domain;
using NodaTime;
using NodaTime.Extensions;
using SequentialGuid;
using ToDo.Api.Models;
using Xunit;

namespace ToDo.Api.Tests
{
	public class ToDoMapperTests : AutoMapperTestBase
	{
		public ToDoMapperTests(AutoMapperFixture fixture) : base(fixture)
		{
		}

		[Fact]
		private void TestToDoPostMappingComplete()
		{
			// Arrange
			var expected = new ToDoTaskPost
			{
				Description = "My ToDo Update",
				DueDate = DateTime.Today.ToString("yyyy-MM-dd")
			};
			// Act
			var actual = Mapper.Map<ToDoTask>(expected);
			// Assert
			Assert.True(actual.Id.ToDateTime().HasValue);
			Assert.Null(actual.Completed);
			Assert.Equal(expected.Description, actual.Description);
			Assert.Equal(
				SystemClock.Instance.InTzdbSystemDefaultZone().GetCurrentDate(),
				actual.DueDate);
		}

		[Fact]
		private void TestToDoPostMappingEmpty()
		{
			// Arrange & Act
			var actual = Mapper.Map<ToDoTask>(new ToDoTaskPost());
			// Assert
			Assert.True(actual.Id.ToDateTime().HasValue);
			Assert.Null(actual.Completed);
			Assert.Null(actual.Description);
			Assert.Null(actual.DueDate);
		}

		[Fact]
		private void TestToDoPostMappingJunk()
		{
			// Arrange
			var expected = new ToDoTaskPost
			{
				Description = "Hello World!",
				DueDate = "Hello World!"
			};
			// Act
			var actual = Mapper.Map<ToDoTask>(expected);
			// Assert
			Assert.True(actual.Id.ToDateTime().HasValue);
			Assert.Null(actual.Completed);
			Assert.Equal(expected.Description, actual.Description);
			Assert.Null(actual.DueDate);
		}

		[Fact]
		private void TestToDoPutMappingComplete()
		{
			// Arrange
			var expectedTimeStamp = DateTime.UtcNow;
			var expectedId =
				SequentialGuidGenerator.Instance.NewGuid(expectedTimeStamp);
			var expected = new ToDoTaskPut
			{
				Completed = "true",
				Description = "My ToDo Update",
				DueDate = DateTime.Today.ToString("yyyy-MM-dd")
			};
			// Act
			var actual = Mapper.Map<ToDoTask>(expectedId, expected);
			// Assert
			Assert.Equal(expectedId, actual.Id);
			Assert.Equal(expectedTimeStamp, actual.Id.ToDateTime());
			Assert.True(actual.Completed);
			Assert.Equal(expected.Description, actual.Description);
			Assert.Equal(
				SystemClock.Instance.InTzdbSystemDefaultZone().GetCurrentDate(),
				actual.DueDate);
		}

		[Fact]
		private void TestToDoPutMappingEmpty()
		{
			// Arrange
			var expectedTimeStamp = DateTime.UtcNow;
			var expectedId =
				SequentialGuidGenerator.Instance.NewGuid(expectedTimeStamp);
			// Act
			var actual = Mapper.Map<ToDoTask>(expectedId, new ToDoTaskPut());
			// Assert
			Assert.Equal(expectedId, actual.Id);
			Assert.Equal(expectedTimeStamp, actual.Id.ToDateTime());
			Assert.Null(actual.Completed);
			Assert.Null(actual.Description);
			Assert.Null(actual.DueDate);
		}

		[Fact]
		private void TestToDoPutMappingJunk()
		{
			// Arrange
			var expectedTimeStamp = DateTime.UtcNow;
			var expectedId =
				SequentialGuidGenerator.Instance.NewGuid(expectedTimeStamp);
			var expected = new ToDoTaskPut
			{
				Completed = "Hello World!",
				Description = "Hello World!",
				DueDate = "Hello World!"
			};
			// Act
			var actual = Mapper.Map<ToDoTask>(expectedId, expected);
			// Assert
			Assert.Equal(expectedId, actual.Id);
			Assert.Equal(expectedTimeStamp, actual.Id.ToDateTime());
			Assert.Null(actual.Completed);
			Assert.Equal(expected.Description, actual.Description);
			Assert.Null(actual.DueDate);
		}
	}
}
