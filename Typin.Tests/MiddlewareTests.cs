namespace Typin.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.Middlewares;
    using Xunit;
    using Xunit.Abstractions;

    public class MiddlewareTests
    {
        private readonly ITestOutputHelper _output;

        public MiddlewareTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Middleware_types_collection_should_contain_all_user_defined_middlewares()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<PipelineCommand>()
                .UseConsole(console)
                .UseMiddleware<ExecutionTimingMiddleware>()
                .UseMiddleware(typeof(ExitCodeMiddleware))
                .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { "pipeline" }, new Dictionary<string, string>());

            // Asert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();

            stdOut.GetString().Should().ContainAll(
                typeof(ExecutionTimingMiddleware).AssemblyQualifiedName,
                typeof(ExitCodeMiddleware).AssemblyQualifiedName,
                PipelineCommand.PipelineTermination,
                "Typin.Internal.Pipeline");

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Middleware_pipeline_should_be_executed()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<PipelineCommand>()
                .UseConsole(console)
                .UseMiddleware<ExecutionTimingMiddleware>()
                .UseMiddleware(typeof(ExitCodeMiddleware))
                .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { }, new Dictionary<string, string>());

            // Asert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();

            stdOut.GetString().Should().ContainAll(
                ExecutionTimingMiddleware.ExpectedOutput0,
                ExecutionTimingMiddleware.ExpectedOutput1,
                ExitCodeMiddleware.ExpectedOutput,
                DefaultCommand.ExpectedOutputText);

            _output.WriteLine(stdOut.GetString());
        }
    }
}