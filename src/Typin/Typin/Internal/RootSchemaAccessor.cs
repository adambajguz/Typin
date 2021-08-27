namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using Typin;
    using Typin.Components;
    using Typin.Internal.Schemas;
    using Typin.Modes;
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

            IReadOnlyList<Type> commands = _componentProvider.Get<ICommand>();
            IReadOnlyList<Type> dynamicCommands = _componentProvider.Get<IDynamicCommand>();
            IReadOnlyList<Type> directives = _componentProvider.Get<IDirective>();
            IReadOnlyList<Type> modes = _componentProvider.Get<ICliMode>();

            if (modes.Count == 0)
            {
                modes = new List<Type>() { typeof(DirectMode) };
            }

            RootSchemaResolver rootSchemaResolver = new(commands, dynamicCommands, directives, modes);

            RootSchema? rootScheam = rootSchemaResolver.Resolve();
            timer.Stop();

            _logger.LogInformation("Root schema resolved after {Duration}.", timer.Elapsed);

            return rootScheam;
        }
    }
}
