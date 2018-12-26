using AutoMapper;
using Common.Api.Mappings;
using ToDo.Api.Mappings;

namespace ToDo.Api.Tests
{
	public class AutoMapperFixture
	{
		public AutoMapperFixture()
		{
			var config = new MapperConfiguration(c =>
			{
				c.AddProfile<SharedMappingProfile>();
				c.AddProfile<ToDoTaskMappingProfile>();
			});
			config.AssertConfigurationIsValid();
			Mapper = config.CreateMapper();
		}

		public IMapper Mapper { get; }

		public const string CollectionName = "AutoMapper";
	}
}
