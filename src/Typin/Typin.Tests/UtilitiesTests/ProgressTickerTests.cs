namespace Typin.Tests.UtilitiesTests
{
    using System.Linq;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Utilities;
    using Xunit;
    using Xunit.Abstractions;

    public class ProgressTickerTests
    {
        private readonly ITestOutputHelper _output;

        public ProgressTickerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Progress_ticker_can_be_used_to_report_progress_to_console()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered(isOutputRedirected: false, isErrorRedirected: false);

            ProgressTicker ticker = console.CreateProgressTicker();

            double[] progressValues = Enumerable.Range(0, 100).Select(p => p / 100.0).ToArray();
            string[] progressStringValues = progressValues.Select(p => p.ToString("P2")).ToArray();

            // Act
            foreach (double progress in progressValues)
                ticker.Report(progress);

            string stdOutData = console.Output.Encoding.GetString(stdOut.GetBytes());

            // Assert
            stdOutData.Should().ContainAll(progressStringValues);

            _output.WriteLine(stdOutData);
        }

        [Fact]
        public void Progress_ticker_does_not_write_to_console_if_output_is_redirected()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            ProgressTicker ticker = console.CreateProgressTicker();

            double[] progressValues = Enumerable.Range(0, 100).Select(p => p / 100.0).ToArray();

            // Act
            foreach (double progress in progressValues)
                ticker.Report(progress);

            string stdOutData = console.Output.Encoding.GetString(stdOut.GetBytes());

            // Assert
            stdOutData.Should().BeEmpty();

            _output.WriteLine(stdOutData);
        }
    }
}