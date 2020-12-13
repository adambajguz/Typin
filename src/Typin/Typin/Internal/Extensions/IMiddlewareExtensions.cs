namespace Typin.Internal.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    internal static class IMiddlewareExtensions
    {
        public static CommandPipelineHandlerDelegate PipelineTermination => () => default;

        public static CommandPipelineHandlerDelegate Next(this IMiddleware commandMiddleware,
                                                          ICliContext cliContext,
                                                          CommandPipelineHandlerDelegate next,
                                                          Type middlewareType,
                                                          ILogger logger,
                                                          CancellationToken cancellationToken)
        {
            return () =>
            {
                logger.LogDebug("Executing middleware {MiddlewareType}", middlewareType.FullName);

                return new ValueTask(commandMiddleware.HandleAsync(cliContext, next, cancellationToken));
            };
        }

        public static CommandPipelineHandlerDelegate Next(this IPipelinedDirective pipelineDirective,
                                                          ICliContext cliContext,
                                                          CommandPipelineHandlerDelegate next,
                                                          ILogger logger,
                                                          CancellationToken cancellationToken)
        {
            return () =>
            {
                logger.LogDebug("Executing pipelined directive {PipelineDirectiveType}.", pipelineDirective.GetType().FullName);

                return pipelineDirective.HandleAsync(cliContext, next, cancellationToken);
            };
        }
    }
}
