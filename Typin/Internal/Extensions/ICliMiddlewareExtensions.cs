namespace Typin.Internal.Extensions
{
    using System.Threading;
    using System.Threading.Tasks;

    internal static class ICliMiddlewareExtensions
    {
        public static CommandPipelineHandlerDelegate PipelineTermination => (context, cancelationToken) => Task.CompletedTask;

        public static CommandPipelineHandlerDelegate Next(this IMiddleware commandMiddleware,
                                                          ICliContext cliContext,
                                                          CommandPipelineHandlerDelegate next,
                                                          CancellationToken cancellationToken)
        {
            return (c, t) => commandMiddleware.HandleAsync(cliContext, next, cancellationToken);
        }
    }
}
