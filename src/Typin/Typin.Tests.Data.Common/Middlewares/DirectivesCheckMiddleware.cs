namespace Typin.Tests.Data.Middlewares
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;

    public sealed class DirectivesCheckMiddleware : IMiddleware
    {
        public const string ExpectedOutput = "Command finished succesfully.";

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            if (context.Input.HasDirective("custom"))
            {
                throw new ApplicationException("custom directive detected");
            }

            await next();
        }
    }
}
