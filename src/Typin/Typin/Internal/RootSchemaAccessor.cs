namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using Typin;
    using Typin.Commands;
    using Typin.Directives;
    using Typin.Hosting.Components;
    using Typin.Internal.Schemas;
    using Typin.Schemas;

    /// <inheritdoc/>
    internal sealed class RootSchemaAccessor : IRootSchemaAccessor
    {
        private readonly Lazy<RootSchema> _rootSchema;

        private readonly IComponentProvider _componentProvider;
        private readonly ILogger _logger;

        ///<inheritdoc/>
        public RootSchema RootSchema => _rootSchema.Value;

        public RootSchemaAccessor(IComponentProvider componentProvider, ILogger<RootSchemaAccessor> logger)
        {
            _componentProvider = componentProvider;
            _logger = logger;

            _rootSchema = new Lazy<RootSchema>(ResolveRootSchema, true);
        }

        private RootSchema ResolveRootSchema()
        {
            _logger.LogDebug("Resolving root schema...");

            Stopwatch timer = Stopwatch.StartNew();

            IReadOnlyCollection<Type> commands = _componentProvider.Get<ICommand>() ?? Array.Empty<Type>();
            IReadOnlyCollection<Type> dynamicCommands = _componentProvider.Get<IDynamicCommand>() ?? Array.Empty<Type>();
            IReadOnlyCollection<Type> directives = _componentProvider.Get<IDirective>() ?? Array.Empty<Type>();

            RootSchemaResolver rootSchemaResolver = new(commands, dynamicCommands, directives);

            RootSchema? rootSchema = rootSchemaResolver.Resolve();
            timer.Stop();

            _logger.LogInformation("Root schema resolved after {Duration}.", timer.Elapsed);

            return rootSchema;
        }
    }
}
