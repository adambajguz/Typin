namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;

    public interface ICommand : ICommand<CommandFinished>
    {

    }

    public interface ICommand<TResult>
    {

    }
}
