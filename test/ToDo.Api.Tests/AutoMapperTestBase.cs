using AutoMapper;
using Xunit;

namespace ToDo.Api.Tests
{
	// Required to get XUnit to drop in valid fixture
	[Collection(AutoMapperFixture.CollectionName)]
	public abstract class AutoMapperTestBase
	{
		protected readonly IMapper Mapper;

		protected AutoMapperTestBase(AutoMapperFixture fixture) =>
			Mapper = fixture.Mapper;
	}
}
