namespace Typin.Tests.Extensions
{
    using Newtonsoft.Json;

    internal static class JsonExtensions
    {
        private static readonly JsonSerializerSettings _settings;

        static JsonExtensions()
        {
            _settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Error,
                DefaultValueHandling = DefaultValueHandling.Include
            };
        }

        public static T DeserializeJson<T>(this string json)
            where T : notnull
        {
            return JsonConvert.DeserializeObject<T>(json, _settings)!;
        }

        public static string SerializeJson<T>(this T obj)
            where T : notnull
        {
            return JsonConvert.SerializeObject(obj, _settings)!;
        }
    }
}