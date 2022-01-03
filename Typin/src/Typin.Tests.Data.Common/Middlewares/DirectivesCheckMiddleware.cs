namespace Typin.Tests.Data.Common.Middlewares
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    public sealed class DirectivesCheckMiddleware : IMiddleware
    {
        public const string ExpectedOutput = "Command finished succesfully.";

        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            if (args.Input.Tokens?.Any(x => x.MatchesName("custom")) ?? false)
            {
                throw new ApplicationException("custom directive detected");
            }

            await next();
        }
    }
}
