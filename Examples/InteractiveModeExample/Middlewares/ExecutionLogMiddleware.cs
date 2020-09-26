namespace InteractiveModeExample.Middlewares
{
    using System.Threading;
    using System.Threading.Tasks;
    using InteractiveModeExample.Services;
    using Typin;

    public sealed class ExecutionLogMiddleware : IMiddleware
    {
        private readonly LibraryService _library;

        public ExecutionLogMiddleware(LibraryService library)
        {
            _library = library;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.Console.Output.WriteLine($"-- Log Command {_library.GetLibrary().Books.Count}");

            await next();
        }
    }
}
