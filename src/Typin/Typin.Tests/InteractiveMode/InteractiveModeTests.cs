namespace Typin.Tests.InteractiveMode
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Modes;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class InteractiveModeTests
    {
        private readonly ITestOutputHelper _output;

        public InteractiveModeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Application_should_print_startup_message_from_simple_string()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<ExitCommand>()
                .UseDirectMode(asStartup:true)
                .UseInteractiveMode();

            string input = $"exit\rexit\nexit\r\n";

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "interactive",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().Contain("dotnet testhost.dll> ");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}