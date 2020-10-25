namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Tests.Data.Commands.Valid;
    using Xunit;
    using Xunit.Abstractions;

    public class ErrorReportingTests
    {
        private readonly ITestOutputHelper _output;

        public ErrorReportingTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Command_may_throw_a_generic_exception_which_exits_and_prints_error_message_and_stack_trace()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<GenericExceptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(new[] { "cmd", "-m", "ErrorTest" });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Should().BeEmpty();
            stdErr.GetString().Should().ContainAll(
                "System.Exception:",
                "ErrorTest", "at",
                "Typin.Tests"
            );

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }


        [Fact]
        public async Task Command_may_throw_a_generic_exception_with_inner_exception_which_exits_and_prints_error_message_and_stack_trace()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<GenericInnerExceptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(new[] { "cmd", "-m", "ErrorTest", "-i", "FooBar" });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Should().BeEmpty();
            stdErr.GetString().Should().ContainAll(
                "System.Exception:",
                "FooBar",
                "ErrorTest", "at",
                "Typin.Tests"
            );

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        [InlineData(126)]
        [InlineData(128)]
        [InlineData(255)]
        [InlineData(-255)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public async Task Command_may_throw_a_specialized_exception_which_exits_with_custom_code_and_prints_minimal_error_details(int errorCode)
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<CommandExceptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(new[] { "cmd", "-m", "ErrorTest", "-c", errorCode.ToString() });

            // Assert
            exitCode.Should().Be(errorCode);
            stdOut.GetString().Should().BeEmpty();
            stdErr.GetString().Trim().Should().Be("ErrorTest");

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_may_throw_a_specialized_exception_without_error_message_which_exits_and_prints_full_error_details()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<CommandExceptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(new[] { "cmd" });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Should().BeEmpty();
            stdErr.GetString().Should().ContainAll(
                "Typin.Exceptions.CommandException:",
                "at",
                "Typin.Tests"
            );

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_may_throw_a_specialized_exception_which_exits_and_prints_help_text()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<CommandExceptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(new[] { "cmd", "-m", "ErrorTest", "--show-help" });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "Usage",
                "Options",
                "-h|--help"
            );
            stdErr.GetString().Trim().Should().Be("ErrorTest");

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_do_not_show_help_text_on_invalid_user_input_with_default_exception_handler()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(new[] { "not-a-valid-command", "-r", "foo" });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Should().NotContainAll(
                "Usage",
                "Options",
                "-h|--help"
            );
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }
    }
}