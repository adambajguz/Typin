namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;

    public sealed class MessageMapping
    {
        public Type MessageType { get; }
        public Type PayloadType { get; }
        public Type? ResultPayloadType { get; }

        public Type HandlerType { get; }
        public Type HandlerInterfaceType { get; }
        public Type HandlerWrapperType { get; }

        public MessageMapping(Type messageType, Type payloadType, Type handlerType, Type handlerInterfaceType, Type handlerWrapperType)
        {
            MessageType = messageType;
            PayloadType = payloadType;
            ResultPayloadType = null;

            HandlerType = handlerType;
            HandlerInterfaceType = handlerInterfaceType;
            HandlerWrapperType = handlerWrapperType;
        }

        public MessageMapping(Type messageType, Type payloadType, Type? resultPayloadType, Type handlerType, Type handlerInterfaceType, Type handlerWrapperType)
        {
            MessageType = messageType;
            PayloadType = payloadType;
            ResultPayloadType = resultPayloadType;

            HandlerType = handlerType;
            HandlerInterfaceType = handlerInterfaceType;
            HandlerWrapperType = handlerWrapperType;
        }
    }
}
