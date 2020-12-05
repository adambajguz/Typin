namespace TypinExamples.Infrastructure.TypinWeb.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Handlers.Commands;
    using TypinExamples.Infrastructure.TypinWeb.Console;
    using TypinExamples.Infrastructure.TypinWeb.Services;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public class WorkerRunExampleHandler : ICommandHandler<RunExampleCommand, RunExampleResult>
    {
        private readonly IWebExampleInvokerService _exampleInvoker;

        public WorkerRunExampleHandler(IWebExampleInvokerService exampleInvoker)
        {
            _exampleInvoker = exampleInvoker;
        }

        public async ValueTask<RunExampleResult> HandleAsync(RunExampleCommand request, IWorker worker, CancellationToken cancellationToken)
        {
            using WebConsole webConsole = new WebConsole(worker, request.TerminalId, cancellationToken);
            _exampleInvoker.AttachConsole(webConsole);
            //_exampleInvoker.AttachLogger(LoggerDestination);

            int exitCode = await _exampleInvoker.Run(request.WebProgramClass, request.Args, request.EnvironmentVariables);

            return new RunExampleResult { ExitCode = exitCode };
        }
    }
}
