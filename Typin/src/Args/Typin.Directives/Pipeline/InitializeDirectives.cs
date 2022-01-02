//namespace Typin.Pipeline
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using Microsoft.Extensions.DependencyInjection;
//    using PackSite.Library.Pipelining;
//    using Typin;
//    using Typin.Exceptions.ArgumentBinding;
//    using Typin.Features;
//    using Typin.Features.Input;
//    using Typin.Schemas;

//    /// <summary>
//    /// Initializes directives.
//    /// </summary>
//    public sealed class InitializeDirectives : IMiddleware
//    {
//        private readonly IRootSchemaAccessor _rootSchemaAccessor;
//        private readonly IServiceProvider _serviceProvider;

//        /// <summary>
//        /// Initializes an instance of <see cref="InitializeDirectives"/>.
//        /// </summary>
//        public InitializeDirectives(IRootSchemaAccessor rootSchemaAccessor, IServiceProvider serviceProvider)
//        {
//            _rootSchemaAccessor = rootSchemaAccessor;
//            _serviceProvider = serviceProvider;
//        }

//        /// <inheritdoc/>
//        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
//        {
//            IBinderFeature binder = args.Binder;

//            //Get current CLI mode and input directives
//            //Type currentModeType = _applicationLifetime.CurrentModeType!;
//            List<DirectiveInput> directives = binder.UnboundedInput.Directives;

//            //Initialize collections
//            List<IDirective> directivesInstances = new();
//            List<IPipelinedDirective> pipelinedDirectivesInstances = new();

//            //Process directive input
//            foreach (DirectiveInput directiveInput in directives)
//            {
//                // Try to get the directive matching the input or fallback to default
//                DirectiveSchema directive = _rootSchemaAccessor.RootSchema.TryFindDirective(directiveInput.Name) ??
//                    throw new UnknownDirectiveInputException(directiveInput);

//                //// Handle interactive directives not supported in current mode
//                //if (!directive.CanBeExecutedInMode(currentModeType))
//                //{
//                //    throw ModeEndUserExceptions.DirectiveExecutedInInvalidMode(directive, currentModeType);
//                //}

//                // Get directive instance
//                IDirective instance = (IDirective)_serviceProvider.GetRequiredService(directive.Type);

//                //Initialize directive
//                await instance.InitializeAsync(cancellationToken);

//                //Add directive to list
//                directivesInstances.Add(instance);

//                if (directive.IsPipelinedDirective && instance is IPipelinedDirective pd)
//                {
//                    pipelinedDirectivesInstances.Add(pd);
//                }
//            }

//            directives.Clear();

//            //Set directives lists in context
//            args.Features.Set<IDirectivesFeature>(new DirectivesFeature(directivesInstances, pipelinedDirectivesInstances));

//            await next();
//        }
//    }
//}
