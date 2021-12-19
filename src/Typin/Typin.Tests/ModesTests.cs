namespace Typin.Tests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Exceptions.Mode;
    using Typin.Modes;
    using Typin.Tests.Data.Common.Extensions;
    using Typin.Tests.Data.Invalid.Modes;
    using Typin.Tests.Data.Valid.DefaultCommands;
    using Typin.Tests.Data.Valid.Modes;
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
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll(DefaultCommand.ExpectedOutputText);
            stdOut.GetString().Should().ContainAll(nameof(ValidCustomMode), nameof(ValidCustomMode) + "END");
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Valid_custom_mode_should_not_throw_error_when_registered_with_typeof()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .RegisterMode(typeof(ValidCustomMode), true);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll(nameof(ValidCustomMode), nameof(ValidCustomMode) + "END");
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Mode_should_not_be_abstract_when_registered_with_typeof()
        {
            // Arrange
            Func<Task> func = async () =>
             {
                 var builder = await new CliApplicationBuilder()
                     .RegisterMode<DirectMode>(asStartup: true)
                     .RegisterMode(typeof(InvalidCustomMode), true)
                     .Build()
                     .RunAsync();
             };

            // Assert
            await func.Should().ThrowAsync<InvalidModeException>().WithMessage("Invalid CLI mode type*");
        }

        [Fact]
        public async Task Mode_should_not_be_abstract()
        {
            // Arrange
            Func<Task> act = async () =>
            {
                var builder = await new CliApplicationBuilder()
                    .RegisterMode<DirectMode>(asStartup: true)
                    .RegisterMode<InvalidAbstractCustomMode>(true)
                    .Build()
                    .RunAsync();
            };

            // Assert
            await act.Should().ThrowAsync<InvalidModeException>().WithMessage("Invalid CLI mode type*");
        }

        [Fact]
        public async Task Invalid_mode_type_should_throw_error()
        {
            // Arrange
            Func<Task> act = async () =>
            {
                var builder = await new CliApplicationBuilder()
                    .RegisterMode<DirectMode>(asStartup: true)
                    .RegisterMode(typeof(InvalidCustomMode), true)
                    .Build()
                    .RunAsync();
            };

            // Assert
            await act.Should().ThrowAsync<InvalidModeException>().WithMessage("Invalid CLI mode type*");
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
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll(DefaultCommand.ExpectedOutputText);
            stdOut.GetString().Should().NotContainAll(nameof(ValidCustomMode), nameof(ValidCustomMode) + "END");
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotContain("not a valid CLI mode type.");
        }

        [Fact]
        public async Task Only_one_startup_mode_can_be_registered()
        {
            // Arrange
            Func<Task> func = async () =>
            {
                var builder = await new CliApplicationBuilder()
                    .RegisterMode<DirectMode>(asStartup: true)
                    .RegisterMode<ValidCustomMode>(true)
                    .Build()
                    .RunAsync();
            };

            // Assert
            await func.Should().ThrowAsync<ArgumentException>().WithMessage("*Only one mode can be registered as startup mode.*");
        }
    }
}