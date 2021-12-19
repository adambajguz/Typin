namespace Typin.Tests.ResolversTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Invalid.Commands;
    using Xunit;
    using Xunit.Abstractions;
    using Typin.Tests.Data.Common.Extensions;
    using Typin.Tests.Data.Valid.Commands;

    public class OptionResolverTests
    {
        private readonly ITestOutputHelper _output;

        public OptionResolverTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Option_alias_should_not_be_available_when_two_dashes_are_specified()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithRequiredOptionsCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, $"{nameof(WithRequiredOptionsCommand)} --a a --c c z \n \b a");

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_option_must_have_valid_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<InvalidOptionNameCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_option_must_have_valid_shortnames()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<InvalidOptionShortNameCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_can_have_names_that_are_empty()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<EmptyOptionNameCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "cmd --help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();

            stdOut.GetString().Should().ContainAll(
                "Options".ToUpperInvariant(),
                "--apple",
                "--blackberries",
                "--west-indian-cherry",
                "--coconut-meat-or-pitaya-dragonfruit",
                "--coconut-meat-or-pitaya");
        }

        [Fact]
        public async Task Command_options_must_have_names_that_are_longer_than_one_character()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SingleCharacterOptionNameCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_have_unique_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateOptionNamesCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_have_unique_short_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateOptionShortNamesCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_have_unique_environment_variable_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateOptionEnvironmentVariableNamesCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_not_have_conflicts_with_the_implicit_help_option()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<ConflictWithHelpOptionCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_not_have_conflicts_with_the_implicit_version_option()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<ConflictWithVersionOptionCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}