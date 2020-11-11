namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using Typin.Console;
    using Typin.Schemas;

    internal class CliContextFactory
    {
        private readonly ApplicationMetadata _metadata;
        private readonly ApplicationConfiguration _configuration;
        private readonly IConsole _console;

        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string>();
        public RootSchema? RootSchema { get; set; }

        public CliContextFactory(ApplicationMetadata metadata,
                                 ApplicationConfiguration configuration,
                                 IConsole console)
        {
            _metadata = metadata;
            _configuration = configuration;
            _console = console;
        }

        public CliContext Create(IServiceProvider serviceProvider)
        {
            if (RootSchema is null)
                throw new NullReferenceException($"Cannot create a new instance of {nameof(CliContext)} because {nameof(RootSchema)} has not been resolved yet.");

            return new CliContext(_metadata, _configuration, RootSchema, EnvironmentVariables, _console);
        }
    }
}