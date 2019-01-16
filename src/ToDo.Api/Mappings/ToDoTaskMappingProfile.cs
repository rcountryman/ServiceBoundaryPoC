using System;
using AutoMapper;
using Common.Domain;
using SequentialGuid;
using ToDo.Api.Models;

namespace ToDo.Api.Mappings
{
	public class ToDoTaskMappingProfile : Profile
	{
		public ToDoTaskMappingProfile()
		{
			// POST should generate an Id
			// POST should ignore completed
			CreateMap<ToDoTaskPost, ToDoTask>()
				.BeforeMap((s, d) =>
					d.Id = SequentialGuidGenerator.Instance.NewGuid())
				.IgnoreMember(d => d.Id)
				.IgnoreMember(d => d.Completed);

			// Create a map from Guid to the id property
			CreateMap<Guid, ToDoTask>()
				.MapFrom(d => d.Id, s => s)
				.IgnoreAllOtherMembers();

			// Direct copy fields except ignore the ID which is in a separate map
			CreateMap<ToDoTaskPut, ToDoTask>()
				.IgnoreMember(d => d.Id);
		}
	}
}
