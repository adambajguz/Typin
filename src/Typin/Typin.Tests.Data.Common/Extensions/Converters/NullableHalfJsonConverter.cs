namespace Typin.Tests.Data.Common.Extensions.Converters
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    internal class NullableHalfJsonConverter : JsonConverter<Half?>
    {
        public override Half? ReadJson(JsonReader reader, Type objectType, Half? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is null)
            {
                return null;
            }

            return Half.Parse((reader.Value as IFormattable)!.ToString(null, CultureInfo.InvariantCulture)!, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, Half? value, JsonSerializer serializer)
        {
            writer.WriteValue((double?)value);
        }
    }
}