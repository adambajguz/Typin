namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Internal.Extensions;

    internal sealed class CliExecutionScope : IDisposable
    {
        public CliContext Context { get; }
        public IServiceScope ServiceScope { get; }

        public CliExecutionScope(CliContext cliContext, IServiceScopeFactory serviceScopeFactory)
        {
            Context = cliContext;
            ServiceScope = serviceScopeFactory.CreateScope();
        }

        public async Task RunPipelineAsync()
        {
            IServiceProvider serviceProvider = ServiceScope.ServiceProvider;
            LinkedList<Type> middlewareTypes = Context.MiddlewareTypes;

            CancellationToken cancellationToken = Context.Console.GetCancellationToken();
            CommandPipelineHandlerDelegate next = IMiddlewareExtensions.PipelineTermination;

            LinkedList<IMiddleware> middlewareComponenets = new LinkedList<IMiddleware>();
            foreach (Type middlewareType in middlewareTypes)
            {
                IMiddleware instance = (IMiddleware)serviceProvider.GetRequiredService(middlewareType);
                next = instance.Next(Context, next, cancellationToken);

                middlewareComponenets.AddFirst(instance);
            }

            await next();
        }

        public void Dispose()
        {
            Context.Input = default!;
            Context.Command = default!;
            Context.CommandDefaultValues = default!;
            Context.CommandSchema = default!;
            Context.ExitCode = null;

            ServiceScope.Dispose();
        }
    }
}
