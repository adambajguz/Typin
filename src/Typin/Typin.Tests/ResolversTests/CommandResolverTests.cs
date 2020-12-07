namespace Typin.Tests.ResolversTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Invalid;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomDirectives.Valid;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class CommandResolverTests
    {
        private readonly ITestOutputHelper _output;

        public CommandResolverTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Invalid_command_type_should_throw_error()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand(typeof(CustomDirective));

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("not a valid command type.");
        }

        [Fact]
        public async Task At_least_one_command_must_be_defined_in_an_application()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .UseStartupMessage("{title} CLI {version} {{title}} {executable} {{{description}}} {test}");

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Commands_must_implement_the_corresponding_interface()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand(typeof(NonImplementedCommand))
                .UseStartupMessage("{title} CLI {version} {{title}} {executable} {{{description}}} {test}");

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Commands_must_be_annotated_by_an_attribute()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NonAnnotatedCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Commands_must_have_unique_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<GenericExceptionCommand>()
                .AddCommand<CommandExceptionCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_can_be_default_but_only_if_it_is_the_only_such_command()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<OtherDefaultCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_option_must_have_valid_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<InvalidOptionNameCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_option_must_have_valid_shortnames()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<InvalidOptionShortNameCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_parameters_must_have_unique_order()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateParameterOrderCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_parameters_must_have_unique_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateParameterNameCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_parameter_can_be_non_scalar_only_if_no_other_such_parameter_is_present()
        {
            var builder = new CliApplicationBuilder()
                .AddCommand<MultipleNonScalarParametersCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_parameter_can_be_non_scalar_only_if_it_is_the_last_in_order()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NonLastNonScalarParameterCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
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
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();

            stdOut.GetString().Should().ContainAll(
                "Options",
                "--apple",
                "--blackberries",
                "--west-indian-cherry",
                "--coconut-meat--or--pitaya-dragonfruit",
                "--coconut-meat-or--pitaya");
        }

        [Fact]
        public async Task Command_options_must_have_names_that_are_longer_than_one_character()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SingleCharacterOptionNameCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_have_unique_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateOptionNamesCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_have_unique_short_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateOptionShortNamesCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_have_unique_environment_variable_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateOptionEnvironmentVariableNamesCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_not_have_conflicts_with_the_implicit_help_option()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<ConflictWithHelpOptionCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_options_must_not_have_conflicts_with_the_implicit_version_option()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<ConflictWithVersionOptionCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}