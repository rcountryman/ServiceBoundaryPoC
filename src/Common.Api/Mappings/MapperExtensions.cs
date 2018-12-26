using System;
using AutoMapper;

namespace Common.Api.Mappings
{
	// Helper methods to make passing predefined ids into the map easier
	public static class MapperExtensions
	{
		public static TDestination Map<TSource, TDestination>(this IMapper mapper, TSource source, Guid id) =>
			mapper.Map<TSource, TDestination>(source, o => o.Items["id"] = id);

		public static TDestination Map<TDestination>(this IMapper mapper, object source, Guid id) =>
			mapper.Map<TDestination>(source, o => o.Items["id"] = id);
	}
}
