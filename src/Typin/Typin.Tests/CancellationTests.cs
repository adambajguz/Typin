namespace Typin.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Tests.Data.Commands.Valid;
    using Xunit;
    using Xunit.Abstractions;

    public class CancellationTests
    {
        private readonly ITestOutputHelper _output;

        public CancellationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Command_can_perform_additional_cleanup_if_cancellation_is_requested()
        {
            // Can't test it with a real console because CliWrap can't send Ctrl+C

            // Arrange
            using var cts = new CancellationTokenSource();
            var (console, stdOut, _) = VirtualConsole.CreateBuffered(cancellationToken: cts.Token);

            var application = new CliApplicationBuilder()
                .AddCommand<CancellableCommand>()
                .UseConsole(console)
                .Build();

            // Act
            cts.CancelAfter(TimeSpan.FromSeconds(0.2));

            int exitCode = await application.RunAsync(new[] { "cmd" });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Trim().Should().Be(CancellableCommand.CancellationOutputText);
        }
    }
}