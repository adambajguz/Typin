namespace Typin.Internal
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using Typin;
    using Typin.Internal.Schemas;
    using Typin.Schemas;

    /// <inheritdoc/>
    internal sealed class RootSchemaAccessor : IRootSchemaAccessor
    {
        private readonly ApplicationConfiguration _configuration;
        private readonly Lazy<RootSchema> _rootSchema;
        private readonly ILogger _logger;

        ///<inheritdoc/>
        public RootSchema RootSchema => _rootSchema.Value;

        public RootSchemaAccessor(ApplicationConfiguration configuration, ILogger<RootSchemaAccessor> logger)
        {
            _rootSchema = new Lazy<RootSchema>(ResolveRootSchema);
            _configuration = configuration;
            _logger = logger;
        }

        private RootSchema ResolveRootSchema()
        {
            _logger.LogDebug("Resolving root schema...");

            Stopwatch timer = new();
            timer.Start();

            RootSchemaResolver rootSchemaResolver = new(_configuration.CommandTypes,
                                                        _configuration.DirectiveTypes,
                                                        _configuration.ModeTypes);

            RootSchema? rootScheam = rootSchemaResolver.Resolve();
            timer.Stop();

            _logger.LogInformation("Root schema resolved after {Duration}.", timer.Elapsed);

            return rootScheam;
        }
    }
}
