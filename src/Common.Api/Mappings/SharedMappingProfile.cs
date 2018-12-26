using AutoMapper;
using NodaTime;
using NodaTime.Text;

namespace Common.Api.Mappings
{
	public class SharedMappingProfile : Profile
	{
		public SharedMappingProfile()
		{
			// Throw an exception on invalid format
			CreateMap<string, bool>()
				.ConvertUsing(s => bool.Parse(s));

			// Gracefully rturn back null on failed parses
			CreateMap<string, bool?>()
				.ConvertUsing(s => ConvertToBool(s));

			// This will throw an exception if the date is null/invalid
			CreateMap<string, LocalDate>()
				.ConvertUsing(s => LocalDatePattern.Iso.Parse(s).Value);

			// Gracefully return back null on failed parses
			CreateMap<string, LocalDate?>()
				.ConvertUsing(s => ConvertToLocalDate(s));
		}

		private static LocalDate? ConvertToLocalDate(string source)
		{
			if (source == null)
				return default;
			var result = LocalDatePattern.Iso.Parse(source);
			return result.Success ? result.Value : default(LocalDate?);
		}

		private static bool? ConvertToBool(string source)
		{
			if (source == null)
				return default;
			return bool.TryParse(source, out var result) ? result : default(bool?);
		}
	}
}
