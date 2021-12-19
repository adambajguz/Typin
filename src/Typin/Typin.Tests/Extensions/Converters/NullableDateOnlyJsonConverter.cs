namespace Typin.Tests.Extensions
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    internal class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
    {
        public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is null)
            {
                return null;
            }

            return DateOnly.Parse(reader.Value as string ?? string.Empty, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.Value.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}