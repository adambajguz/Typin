namespace Typin.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Tests.Commands.Valid;
    using Xunit;

    public class CancellationTests
    {
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
            exitCode.Should().NotBe(0);
            stdOut.GetString().Trim().Should().Be(CancellableCommand.CancellationOutputText);
        }
    }
}