namespace TypinExamples.Domain.Interfaces.Handlers.Core
{
    using MediatR;

    public interface ICoreRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : ICoreRequest<TResponse>
    {

    }

    public interface ICoreRequestHandler<in TRequest> : IRequestHandler<TRequest>
        where TRequest : ICoreRequest<Unit>
    {

    }
}
