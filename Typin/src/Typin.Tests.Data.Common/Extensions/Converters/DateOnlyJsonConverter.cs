namespace Typin.Tests.Data.Common.Extensions.Converters
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    internal class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return DateOnly.Parse(reader.Value as string ?? string.Empty, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}