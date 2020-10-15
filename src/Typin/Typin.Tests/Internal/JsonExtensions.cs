namespace Typin.Tests.Internal
{
    using Newtonsoft.Json;

    internal static class JsonExtensions
    {
        public static T DeserializeJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}