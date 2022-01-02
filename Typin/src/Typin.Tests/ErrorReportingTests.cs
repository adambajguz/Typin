namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Common.Extensions;
    using Typin.Tests.Data.Invalid.DuplicatedCommands;
    using Typin.Tests.Data.Valid.DefaultCommands;
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
            var builder = new CliApplicationBuilder()
                .AddCommand<GenericExceptionCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "duplicated-ex", "-m", "ErrorTest" });

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeEmpty();
            stdErr.GetString().Should().ContainAll(
                "System.Exception:",
                "ErrorTest", "at",
                "Typin.Tests"
            );
        }


        [Fact]
        public async Task Command_may_throw_a_generic_exception_with_inner_exception_which_exits_and_prints_error_message_and_stack_trace()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<GenericInnerExceptionCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "duplicated-ex", "-m", "ErrorTest", "-i", "FooBar" });

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeEmpty();
            stdErr.GetString().Should().ContainAll(
                "System.Exception:",
                "FooBar",
                "ErrorTest", "at",
                "Typin.Tests"
            );
        }

        [Fact]
        public async Task Command_do_not_show_help_text_on_invalid_user_input_with_default_exception_handler()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "not-a-valid-command", "-r", "foo" });

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().NotContainAll(
                "Usage",
                "Options",
                "-h|--help"
            );
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}