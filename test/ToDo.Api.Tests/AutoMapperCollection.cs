using Xunit;

namespace ToDo.Api.Tests
{
	// Required to get XUnit to drop in valid fixture
	[CollectionDefinition(AutoMapperFixture.CollectionName)]
	public class AutoMapperCollection : ICollectionFixture<AutoMapperFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
