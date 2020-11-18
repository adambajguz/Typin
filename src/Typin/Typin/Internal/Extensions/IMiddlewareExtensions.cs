namespace Typin.Internal.Extensions
{
    using System.Threading;
    using System.Threading.Tasks;

    internal static class IMiddlewareExtensions
    {
        public static CommandPipelineHandlerDelegate PipelineTermination => () => default;

        public static CommandPipelineHandlerDelegate Next(this IMiddleware commandMiddleware,
                                                          ICliContext cliContext,
                                                          CommandPipelineHandlerDelegate next,
                                                          CancellationToken cancellationToken)
        {
            return () => new ValueTask(commandMiddleware.HandleAsync(cliContext, next, cancellationToken));
        }

        public static CommandPipelineHandlerDelegate Next(this IPipelinedDirective pipelineDirective,
                                                          ICliContext cliContext,
                                                          CommandPipelineHandlerDelegate next,
                                                          CancellationToken cancellationToken)
        {
            return () => pipelineDirective.HandleAsync(cliContext, next, cancellationToken);
        }
    }
}
