using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace Common.Domain
{
	public static class MappingExtensions
	{
		// Map in the options without needing to deal with all the noise
		public static TDestination Map<TDestination>(this IMapper mapper,
			object source, IReadOnlyDictionary<string, object> options) =>
			mapper.Map<TDestination>(source, o =>
			{
				foreach (var opt in options)
					o.Items.Add(opt);
			});

		public static T Map<T>(this IMapper mapper, params object[] sources)
			where T : new() => sources.Aggregate(new T(),
			(current, source) => mapper.Map(source, current));

		// Run map from without all the excess lambda noise
		public static IMappingExpression<TSource, TDestination>
			MapFrom<TSource, TDestination, TSourceMember>(
				this IMappingExpression<TSource, TDestination> map,
				Expression<Func<TDestination, TSourceMember>> destinationMember,
				Expression<Func<TSource, TSourceMember>> sourceMember) =>
			map.ForMember(destinationMember, o => o.MapFrom(sourceMember));

		public static IMappingExpression
			MapFrom<TSourceMember>(
				this IMappingExpression map,
				string destinationMember,
				Expression<Func<object, TSourceMember>> sourceMember) =>
			map.ForMember(destinationMember, o => o.MapFrom(sourceMember));

		// Run ignore member without all the excess lambda noise
		public static IMappingExpression<TSource, TDestination>
			IgnoreMember<TSource, TDestination, TSourceMember>(
				this IMappingExpression<TSource, TDestination> map,
				Expression<Func<TDestination, TSourceMember>>
					destinationMember) =>
			map.ForMember(destinationMember, o => o.Ignore());

		// Run ignore all members without all the excess lambda noise
		public static void IgnoreAllMembers<TSource, TDestination>(
			this IMappingExpression<TSource, TDestination> map) =>
			map.ForAllMembers(o => o.Ignore());

		public static void IgnoreAllMembers(this IMappingExpression map) =>
			map.ForAllMembers(o => o.Ignore());

		public static void IgnoreAllOtherMembers<TSource, TDestination>(
			this IMappingExpression<TSource, TDestination> map) =>
			map.ForAllOtherMembers(o => o.Ignore());

		public static void IgnoreAllOtherMembers(this IMappingExpression map) =>
			map.ForAllOtherMembers(o => o.Ignore());
	}
}
