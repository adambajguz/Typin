namespace TypinExamples.Domain.Interfaces.Handlers.Core
{
    using MediatR;

    public interface ICoreRequest : IBaseRequest
    {

    }

    public interface ICoreRequest<out TResponse> : ICoreRequest, IRequest<TResponse>
    {

    }
}
