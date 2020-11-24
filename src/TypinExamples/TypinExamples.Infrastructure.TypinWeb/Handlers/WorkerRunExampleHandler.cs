namespace TypinExamples.Infrastructure.TypinWeb.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Handlers.Commands;
    using TypinExamples.Application.Services.Workers;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;
    using TypinExamples.Domain.Models.Workers;
    using TypinExamples.Infrastructure.TypinWeb.Console;
    using TypinExamples.Infrastructure.TypinWeb.Services;
    public static class TaskExtensions
    {
        public static async Task<T> WaitOrCancel<T>(this Task<T> task, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await Task.WhenAny(task, token.WhenCanceled());
            token.ThrowIfCancellationRequested();

            return await task;
        }

        public static Task WhenCanceled(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }
    }

    public class WorkerRunExampleHandler : IWorkerRequestHandler<RunExampleCommand>
    {
        private readonly IWebExampleInvokerService _exampleInvoker;
        private readonly ICoreMessageDispatcher _coreMessageDispatcher;

        public WorkerRunExampleHandler(IWebExampleInvokerService exampleInvoker, ICoreMessageDispatcher coreMessageDispatcher)
        {
            _exampleInvoker = exampleInvoker;
            _coreMessageDispatcher = coreMessageDispatcher;
        }

        public async Task<WorkerResult> Handle(RunExampleCommand request, CancellationToken cancellationToken)
        {
            WebConsole webConsole = new WebConsole(_coreMessageDispatcher, request.TerminalId);
            _exampleInvoker.AttachConsole(webConsole);
            //_exampleInvoker.AttachLogger(LoggerDestination);

            await _exampleInvoker.Run(request.WebProgramClass, request.Args, new Dictionary<string, string>()).WaitOrCancel(cancellationToken);

            return new WorkerResult { Data = $"Processed by WorkerPingHandler {request.TerminalId}" };
        }
    }
}
