namespace Typin.Internal
{
    using System;
    using Typin.Console;
    using Typin.Schemas;

    internal class CliContextFactory
    {
        private readonly ApplicationMetadata _metadata;
        private readonly ApplicationConfiguration _configuration;
        private readonly RootSchema _rootSchema;
        private readonly IConsole _console;

        public CliContextFactory(ApplicationMetadata metadata,
                                 ApplicationConfiguration configuration,
                                 RootSchema rootSchema,
                                 IConsole console)
        {
            _metadata = metadata;
            _configuration = configuration;
            _rootSchema = rootSchema;
            _console = console;
        }

        public CliContext Create(IServiceProvider _)
        {
            return new CliContext(_metadata, _configuration, _rootSchema, _console);
        }
    }
}