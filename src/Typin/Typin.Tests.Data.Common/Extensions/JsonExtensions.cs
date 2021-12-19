namespace Typin.Tests.Extensions
{
    using Newtonsoft.Json;

    public static class JsonExtensions
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

            _settings.Converters.Add(new NullableHalfJsonConverter());
            _settings.Converters.Add(new HalfJsonConverter());

            _settings.Converters.Add(new NullableDateOnlyJsonConverter());
            _settings.Converters.Add(new DateOnlyJsonConverter());

            _settings.Converters.Add(new NullableTimeOnlyJsonConverter());
            _settings.Converters.Add(new TimeOnlyJsonConverter());
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