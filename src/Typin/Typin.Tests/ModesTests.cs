namespace Typin.Tests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Modes;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.Modes.Valid;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class ModesTests
    {
        private readonly ITestOutputHelper _output;

        public ModesTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Valid_custom_mode_should_not_throw_error()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .RegisterMode<ValidCustomMode>(true);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(DefaultCommand.ExpectedOutputText);
            stdOut.GetString().Should().ContainAll(nameof(ValidCustomMode), nameof(ValidCustomMode) + "END");
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Valid_custom_mode_should_not_throw_error_when_registered_withe_typeof()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .RegisterMode(typeof(ValidCustomMode), true);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(nameof(ValidCustomMode), nameof(ValidCustomMode) + "END");
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Invalid_mode_type_should_throw_error()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .RegisterMode(typeof(InvalidCustomMode), true);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("not a valid CLI mode type.");
        }

        [Fact]
        public async Task When_no_startup_mode_was_registered_direct_mode_should_be_executed()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .RegisterMode<ValidCustomMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(DefaultCommand.ExpectedOutputText);
            stdOut.GetString().Should().NotContainAll(nameof(ValidCustomMode), nameof(ValidCustomMode) + "END");
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotContain("not a valid CLI mode type.");
        }

        [Fact]
        public void Only_one_startup_mode_can_be_registered()
        {
            // Arrange
            Action act = () =>
            {
                var builder = new CliApplicationBuilder()
                                 .UseDirectMode(true)
                                 .RegisterMode<ValidCustomMode>(true);
            };

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*Only one mode can be registered as startup mode.*");
        }
    }
}