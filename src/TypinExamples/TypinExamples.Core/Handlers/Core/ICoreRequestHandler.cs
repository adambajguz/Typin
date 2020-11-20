namespace TypinExamples.Core.Handlers.Core
{
    using MediatR;
    using TypinExamples.Core.Models;

    public interface ICoreRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : ICoreRequest<TResponse>
    {

    }
}
