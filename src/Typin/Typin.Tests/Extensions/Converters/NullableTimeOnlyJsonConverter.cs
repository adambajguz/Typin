namespace Typin.Tests.Extensions
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    internal class NullableTimeOnlyJsonConverter : JsonConverter<TimeOnly?>
    {
        private const string TimeFormat = "HH:mm:ss.FFFFFFF";

        public override TimeOnly? ReadJson(JsonReader reader, Type objectType, TimeOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is null)
            {
                return null;
            }

            return TimeOnly.ParseExact(reader.Value as string ?? string.Empty, TimeFormat, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, TimeOnly? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.Value.ToString(TimeFormat, CultureInfo.InvariantCulture));
            }
        }
    }
}