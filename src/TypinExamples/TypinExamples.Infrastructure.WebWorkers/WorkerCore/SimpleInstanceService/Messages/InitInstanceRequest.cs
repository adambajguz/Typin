namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService.Messages
{
    using Newtonsoft.Json;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService;

    public class InitInstanceRequest
    {

        public static readonly string Prefix = $"{SimpleInstanceService.MessagePrefix}{SimpleInstanceService.InitInstanceMessagePrefix}";
        public long CallId { get; set; }
        public long Id { get; set; }
        public string TypeName { get; set; }
        public string AssemblyName { get; set; }

        internal static bool CanDeserialize(string initMessage)
        {
            return initMessage.StartsWith(Prefix);
        }

        internal static InitInstanceRequest Deserialize(string initMessage)
        {
            var result = JsonConvert.DeserializeObject<InitInstanceRequest>(initMessage);
            return result;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
