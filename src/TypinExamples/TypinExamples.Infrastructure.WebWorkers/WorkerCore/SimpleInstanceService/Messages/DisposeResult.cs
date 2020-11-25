namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService.Messages
{
    using System;
    using Newtonsoft.Json;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService;

    public class DisposeResult
    {
        public static readonly string Prefix = $"{SimpleInstanceService.MessagePrefix}{SimpleInstanceService.DiposeResultMessagePrefix}";

        public long CallId { get; set; }

        public bool IsSuccess { get; set; }
        public long InstanceId { get; set; }

        public Exception Exception { get; internal set; }

        internal string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static bool CanDeserialize(string message)
        {
            return message.StartsWith(Prefix);
        }

        public static DisposeResult Deserialize(string message)
        {
            var result = JsonConvert.DeserializeObject<DisposeResult>(message);

            return result;
        }
    }
}