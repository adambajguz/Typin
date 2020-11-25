namespace BlazorWorker.WorkerBackgroundService
{
    using System;
    using System.Collections.Generic;

    public class MessageHandlerRegistry : Dictionary<string, Action<string>>
    {
        public MessageHandlerRegistry(ISerializer messageSerializer)
        {
            MessageSerializer = messageSerializer;
        }

        public ISerializer MessageSerializer { get; }

        public void Add<T>(Action<T> messageHandler) where T : BaseMessage
        {
            base.Add(typeof(T).Name, message => messageHandler(MessageSerializer.Deserialize<T>(message)));
        }

        public bool HandleMessage(string message)
        {
            string key = GetMessageType(message);

            if (key is null)
                return true;

            if (base.TryGetValue(key, out var handler))
            {
                handler(message);
                return true;
            }

            return false;
        }

        public bool HandlesMessage(string message)
        {
            var key = GetMessageType(message);
            return base.ContainsKey(key);
        }

        private string GetMessageType(string message)
        {
            return MessageSerializer.Deserialize<BaseMessage>(message).MessageType;
        }
    }
}

