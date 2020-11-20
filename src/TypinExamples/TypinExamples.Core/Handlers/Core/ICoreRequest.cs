namespace TypinExamples.Core.Handlers.Core
{
    using MediatR;

    public interface ICoreRequest : IBaseRequest
    {

    }

    public interface ICoreRequest<out TResponse> : ICoreRequest, IRequest<TResponse>
    {

    }
}
