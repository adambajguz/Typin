namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService.Messages
{
    using Newtonsoft.Json;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService;

    public class DisposeInstanceRequest
    {
        public static readonly string Prefix =
            $"{SimpleInstanceService.MessagePrefix}{SimpleInstanceService.DiposeMessagePrefix}";

        public long CallId { get; set; }

        public long InstanceId { get; set; }

        public static bool CanDeserialize(string message)
        {
            return message.StartsWith(Prefix);
        }

        public static DisposeInstanceRequest Deserialize(string message)
        {
            var result = JsonConvert.DeserializeObject<DisposeInstanceRequest>(message);

            return result;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
