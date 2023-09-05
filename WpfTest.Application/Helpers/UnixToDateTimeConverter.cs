using System;
using Newtonsoft.Json;

namespace WpfTest.Application.Helpers
{
	public class UnixToDateTimeConverter : JsonConverter<DateTime>
	{
		public bool? IsFormatInSeconds { get; set; }

		private static readonly long UnixMinSeconds = DateTimeOffset.MinValue.ToUnixTimeSeconds(); // -62_135_596_800
		private static readonly long UnixMaxSeconds = DateTimeOffset.MaxValue.ToUnixTimeSeconds(); // 253_402_300_799

		public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}

		public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			try
			{
				long? time = (long?)reader.Value;

				if (time is null)
				{
					// Return MinValue if the json value cannot be cast to long
					return DateTime.MinValue;
				}

				// If 'IsFormatInSeconds' is unspecified, then deduce the correct type based on whether it can be
				// represented as seconds within the .net DateTime min/max range (1/1/0001 to 31/12/9999).
				// Because we're dealing with a 64bit value, the unix time in seconds can exceed the traditional 32bit min/max restrictions (1/1/1970 to 19/1/2038)
				if (IsFormatInSeconds == true || IsFormatInSeconds == null && time > UnixMinSeconds && time < UnixMaxSeconds)
				{
					return DateTimeOffset.FromUnixTimeSeconds(time.Value).LocalDateTime;
				}

				return DateTimeOffset.FromUnixTimeMilliseconds(time.Value).LocalDateTime;
			}
			catch
			{
				// If something goes wrong when casting to long (if the token isn't a number)... we ignore it and return MinValue
			}
        
			return DateTime.MinValue;
		}
	}
}
