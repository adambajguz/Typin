namespace TypinExamples.Infrastructure.TypinWeb.Handlers.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Handlers.Commands;
    using TypinExamples.Infrastructure.TypinWeb.Services;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public class WorkerRunExampleHandler : ICommandHandler<RunExampleCommand, RunExampleResult>
    {
        public async ValueTask<RunExampleResult> HandleAsync(RunExampleCommand request, IWorker worker, CancellationToken cancellationToken)
        {
            _ = request.TerminalId ?? throw new ArgumentNullException(nameof(request), $"{nameof(request.TerminalId)} is null");
            _ = request.WebProgramClass ?? throw new ArgumentNullException(nameof(request), $"{nameof(request.WebProgramClass)} is null");
            _ = request.Args ?? throw new ArgumentNullException(nameof(request), $"{nameof(request.Args)} is null");

            using (WebExampleInvoker exampleInvoker = new(worker, request.TerminalId, cancellationToken))
            {
                int exitCode = await exampleInvoker.Run(request.WebProgramClass, request.Args, request.EnvironmentVariables);

                return new RunExampleResult { ExitCode = exitCode };
            }
        }
    }
}
