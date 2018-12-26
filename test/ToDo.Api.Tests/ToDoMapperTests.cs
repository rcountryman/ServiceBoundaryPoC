using System;
using AutoMapper;
using Common.Api.Mappings;
using NodaTime;
using NodaTime.Extensions;
using SequentialGuid;
using ToDo.Api.Models;
using Xunit;

namespace ToDo.Api.Tests
{
	// Required to get XUnit to drop in valid fixture
	[Collection(AutoMapperFixture.CollectionName)]
	public class ToDoMapperTests
	{
		private readonly IMapper _mapper;

		public ToDoMapperTests(AutoMapperFixture fixture) =>
			_mapper = fixture.Mapper;

		[Fact]
		private void TestToDoPostMappingComplete()
		{
			// Arrange
			var source = new ToDoTaskPost
			{
				Description = "My ToDo Update",
				DueDate = DateTime.Today.ToString("yyyy-MM-dd")
			};
			// Act
			var destination = _mapper.Map<ToDoTask>(source);
			// Assert
			Assert.True(destination.Id.ToDateTime().HasValue);
			Assert.Null(destination.Completed);
			Assert.Equal(source.Description, destination.Description);
			Assert.Equal(
				SystemClock.Instance.InTzdbSystemDefaultZone().GetCurrentDate(),
				destination.DueDate);
		}

		[Fact]
		private void TestToDoPostMappingEmpty()
		{
			// Arrange
			var source = new ToDoTaskPost();
			// Act
			var destination = _mapper.Map<ToDoTask>(source);
			// Assert
			Assert.True(destination.Id.ToDateTime().HasValue);
			Assert.Null(destination.Completed);
			Assert.Null(destination.Description);
			Assert.Null(destination.DueDate);
		}

		[Fact]
		private void TestToDoPostMappingJunk()
		{
			// Arrange
			var source = new ToDoTaskPost
			{
				Description = "Hello World!",
				DueDate = "Hello World!"
			};
			// Act
			var destination = _mapper.Map<ToDoTask>(source);
			// Assert
			Assert.True(destination.Id.ToDateTime().HasValue);
			Assert.Null(destination.Completed);
			Assert.Equal(source.Description, destination.Description);
			Assert.Null(destination.DueDate);
		}

		[Fact]
		private void TestToDoPutMappingComplete()
		{
			// Arrange
			var timeStamp = DateTime.UtcNow;
			var id = SequentialGuidGenerator.Instance.NewGuid(timeStamp);
			var source = new ToDoTaskPut
			{
				Completed = "true",
				Description = "My ToDo Update",
				DueDate = DateTime.Today.ToString("yyyy-MM-dd")
			};
			// Act
			var destination = _mapper.Map<ToDoTask>(source, id);
			// Assert
			Assert.Equal(timeStamp, destination.Id.ToDateTime());
			Assert.True(destination.Completed);
			Assert.Equal(source.Description, destination.Description);
			Assert.Equal(
				SystemClock.Instance.InTzdbSystemDefaultZone().GetCurrentDate(),
				destination.DueDate);
		}

		[Fact]
		private void TestToDoPutMappingEmpty()
		{
			// Arrange
			var timeStamp = DateTime.UtcNow;
			var id = SequentialGuidGenerator.Instance.NewGuid(timeStamp);
			var source = new ToDoTaskPut();
			// Act
			var destination = _mapper.Map<ToDoTask>(source, id);
			// Assert
			Assert.Equal(timeStamp, destination.Id.ToDateTime());
			Assert.Null(destination.Completed);
			Assert.Null(destination.Description);
			Assert.Null(destination.DueDate);
		}

		[Fact]
		private void TestToDoPutMappingJunk()
		{
			// Arrange
			var timeStamp = DateTime.UtcNow;
			var id = SequentialGuidGenerator.Instance.NewGuid(timeStamp);
			var source = new ToDoTaskPut
			{
				Completed = "Hello World!",
				Description = "Hello World!",
				DueDate = "Hello World!"
			};
			// Act
			var destination = _mapper.Map<ToDoTask>(source, id);
			// Assert
			Assert.Equal(timeStamp, destination.Id.ToDateTime());
			Assert.Null(destination.Completed);
			Assert.Equal(source.Description, destination.Description);
			Assert.Null(destination.DueDate);
		}
	}
}
