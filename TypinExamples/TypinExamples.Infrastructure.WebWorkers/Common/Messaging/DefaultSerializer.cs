namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging
{
    using Newtonsoft.Json;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public class DefaultSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public DefaultSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                MissingMemberHandling = MissingMemberHandling.Error,
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T Deserialize<T>(string json)
            where T : notnull
        {
            return JsonConvert.DeserializeObject<T>(json, _settings)!;
        }
    }
}
