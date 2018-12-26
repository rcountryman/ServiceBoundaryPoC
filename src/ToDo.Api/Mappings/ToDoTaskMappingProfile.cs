using System;
using AutoMapper;
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
				.BeforeMap((s, d) => d.Id = SequentialGuidGenerator.Instance.NewGuid())
				.ForMember(d => d.Id, o => o.Ignore())
				.ForMember(d => d.Completed, o => o.Ignore());

			// PUT shoud pull the Id from the Items dictionary
			CreateMap<ToDoTaskPut, ToDoTask>()
				.BeforeMap((s, d, o) => d.Id = new Guid(o.Items["id"].ToString()))
				.ForMember(d => d.Id, o => o.Ignore());
		}
	}
}
