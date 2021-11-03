namespace Typin.Tests.Data.Middlewares
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    public sealed class DirectivesCheckMiddleware : IMiddleware
    {
        public const string ExpectedOutput = "Command finished succesfully.";

        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            if (args.Input?.HasDirective("custom") ?? false)
            {
                throw new ApplicationException("custom directive detected");
            }

            await next();
        }
    }
}
