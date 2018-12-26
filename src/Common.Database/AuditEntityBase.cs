using System;
using System.Net;
using NodaTime;
using NodaTime.TimeZones;
using SequentialGuid;

namespace Common.Database
{
	public abstract class AuditEntityBase<T>
	{
		private Guid _id;
		private DateTimeZone _timeZone;

		public Guid Id
		{
			get => _id;
			set
			{
				_id = value;
				Timestamp = _id.ToDateTime();
				SetLocalTime();
			}
		}

		public IPAddress IpAddress { get; set; }

		public string TimeZone
		{
			get => _timeZone?.Id;
			set
			{
				if (value == default)
					_timeZone = default;
				else
					try
					{
						_timeZone = DateTimeZoneProviders.Tzdb[value];
					}
					catch (DateTimeZoneNotFoundException)
					{
						_timeZone = default;
					}

				SetLocalTime();
			}
		}

		public DateTime? Timestamp { get; private set; }

		public DateTimeOffset? LocalTime { get; private set; }

		public T Entity { get; set; }

		private void SetLocalTime() => LocalTime =
			!Timestamp.HasValue || _timeZone == default
				? default(DateTimeOffset?)
				: new ZonedDateTime(Instant.FromDateTimeUtc(Timestamp.Value),
					_timeZone).ToDateTimeOffset();
	}
}
