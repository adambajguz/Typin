namespace Typin.Tests.Extensions
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    internal class HalfJsonConverter : JsonConverter<Half>
    {
        public override Half ReadJson(JsonReader reader, Type objectType, Half existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Half.Parse((reader.Value as IFormattable)!.ToString(null, CultureInfo.InvariantCulture)!, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, Half value, JsonSerializer serializer)
        {
            writer.WriteValue((double)value);
        }
    }
}