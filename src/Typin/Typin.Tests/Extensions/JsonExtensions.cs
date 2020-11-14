namespace Typin.Tests.Extensions
{
    using Newtonsoft.Json;

    internal static class JsonExtensions
    {
        public static T DeserializeJson<T>(this string json, JsonSerializerSettings? settings = null)
            where T : notnull
        {
            return JsonConvert.DeserializeObject<T>(json, settings)!;
        }
    }
}