namespace TypinExamples.Domain.Interfaces.Handlers.Core
{
    using MediatR;

    public interface ICoreRequest : IBaseRequest, IWorkerIdentifiable
    {

    }

    public interface ICoreRequest<out TResponse> : ICoreRequest, IRequest<TResponse>
    {

    }
}
