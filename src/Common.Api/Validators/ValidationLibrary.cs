using System.Text.RegularExpressions;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Text;

namespace Common.Api.Validators
{
	public static class ValidationLibrary
	{
		public const string
			AtLeastOneRequired = "must specify at least one property",
			BooleanError = "must be either true or false",
			FutureDateError = "cannot be in the past",
			RequiredError = "required",
			ValidDateError = "must be in ISO 8601 format yyyy-MM-dd";

		public static bool IsBoolean(this string value) =>
			Regex.IsMatch(value, @"^(true|false)$", RegexOptions.IgnoreCase);

		public static bool IsFutureDate(this string value)
		{
			var parseResult = LocalDatePattern.Iso.Parse(value);
			if (!parseResult.Success)
				return false;
			return parseResult.Value >= SystemClock.Instance
					   .InTzdbSystemDefaultZone()
					   .GetCurrentDate();
		}

		public static bool IsValidDate(this string value) =>
			LocalDatePattern.Iso.Parse(value).Success;
	}
}
