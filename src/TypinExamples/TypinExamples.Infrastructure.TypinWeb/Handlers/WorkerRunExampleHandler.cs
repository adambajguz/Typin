namespace TypinExamples.Infrastructure.TypinWeb.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Handlers.Commands;
    using TypinExamples.Infrastructure.TypinWeb.Console;
    using TypinExamples.Infrastructure.TypinWeb.Services;

    public class WorkerRunExampleHandler : IWorkerRequestHandler<RunExampleCommand>
    {
        private readonly IWebExampleInvokerService _exampleInvoker;

        public WorkerRunExampleHandler(IWebExampleInvokerService exampleInvoker, ICoreMessageDispatcher coreMessageDispatcher)
        {
            _exampleInvoker = exampleInvoker;
            _coreMessageDispatcher = coreMessageDispatcher;
        }

        public async Task Handle(RunExampleCommand request, CancellationToken cancellationToken)
        {
            using WebConsole webConsole = new WebConsole(_coreMessageDispatcher, request.TerminalId);
            _exampleInvoker.AttachConsole(webConsole);
            //_exampleInvoker.AttachLogger(LoggerDestination);

            await _exampleInvoker.Run(request.WebProgramClass, request.Args, new Dictionary<string, string>());
        }
    }
}
