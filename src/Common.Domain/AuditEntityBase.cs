using System;
using System.Net;
using NodaTime;
using NodaTime.TimeZones;
using SequentialGuid;

namespace Common.Domain
{
	public class AuditEntityBase<T>
	{
		private Guid _id;
		private DateTimeZone _timeZone;

		public Guid Id
		{
			get => _id;
			set
			{
				_id = value;
				var time = _id.ToDateTime();

				Timestamp = time.HasValue
					? Instant.FromDateTimeUtc(time.Value)
					: default(Instant?);
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

		public Instant? Timestamp { get; private set; }

		public ZonedDateTime? LocalTime { get; private set; }

		public T Entity { get; set; }

		private void SetLocalTime() => LocalTime =
			!Timestamp.HasValue || _timeZone == default
				? default(ZonedDateTime?)
				: new ZonedDateTime(Timestamp.Value, _timeZone);
	}
}
