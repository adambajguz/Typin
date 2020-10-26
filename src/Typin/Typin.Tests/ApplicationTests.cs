namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Invalid;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class ApplicationTests
    {
        private readonly ITestOutputHelper _output;

        public ApplicationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(new string[] { "--str", "hello \\ world", "-i", "13", "-b" }, "{\"StrOption\":\"hello \\\\ world\",\"IntOption\":13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello \\ world", "-i", "13", "-b" }, "{\"StrOption\":\"hello \\\\ world\",\"IntOption\":13,\"BoolOption\":true}", true)]
        [InlineData(new string[] { "--str", "hello \" world", "-i", "13", "-b" }, "{\"StrOption\":\"hello \\\" world\",\"IntOption\":13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello \" world", "-i", "13", "-b" }, "{\"StrOption\":\"hello \\\" world\",\"IntOption\":13,\"BoolOption\":true}", true)]
        [InlineData(new string[] { "--str", "hello world", "-i", "13", "-b" }, "{\"StrOption\":\"hello world\",\"IntOption\":13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello world", "-i", "13", "-b" }, "{\"StrOption\":\"hello world\",\"IntOption\":13,\"BoolOption\":true}", true)]
        [InlineData(new string[] { "--str", "hello world", "-i", "-13", "-b" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello world", "-i", "-13", "-b" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true)]
        public async Task Application_can_be_created_and_executed_with_list_command(string[] commandLineArguments, string result, bool interactive)
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>();

            if (interactive)
                builder.UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, commandLineArguments);

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().ContainEquivalentOf(result);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("--str \"hello \\ world\" -i 13 -b", "{\"StrOption\":\"hello \\\\ world\",\"IntOption\":13,\"BoolOption\":true}", false)]
        [InlineData("--str \"hello \\ world\" -i 13 -b", "{\"StrOption\":\"hello \\\\ world\",\"IntOption\":13,\"BoolOption\":true}", true)]
        [InlineData("--str \"hello \\\" world\" -i 13 -b", "{\"StrOption\":\"hello \\\" world\",\"IntOption\":13,\"BoolOption\":true}", false)]
        [InlineData("--str \"hello \\\" world\" -i 13 -b", "{\"StrOption\":\"hello \\\" world\",\"IntOption\":13,\"BoolOption\":true}", true)]
        [InlineData("--str \"hello world\" -i 13 -b", "{\"StrOption\":\"hello world\",\"IntOption\":13,\"BoolOption\":true}", false)]
        [InlineData("--str \"hello world\" -i 13 -b", "{\"StrOption\":\"hello world\",\"IntOption\":13,\"BoolOption\":true}", true)]
        [InlineData("--str \"hello world\" -i -13 -b", "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false)]
        [InlineData("--str \"hello world\" -i -13 -b", "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true)]
        [InlineData("--str \"hello world\" -i \"-13\" \"-b\"", "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false)]
        [InlineData("--str \"hello world\" -i \"-13\" \"-b\"", "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true)]
        public async Task Application_can_be_created_and_executed_with_string_command(string commandLine, string result, bool interactive)
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>();

            if (interactive)
                builder.UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, commandLine, isInputRedirected: interactive);

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().ContainEquivalentOf(result);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Application_without_interactive_mode_cannot_execute_interactive_only_commands()
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>()
                                                     .AddCommand<NamedInteractiveOnlyCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new string[] { "named-interactive-only" }, isInputRedirected: false);

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("can be executed only in interactive mode, but this application is using CliApplication.");
        }

        [Fact]
        public async Task Application_without_interactive_mode_cannot_execute_interactive_only_commands_even_if_supports_interactive_mode_but_is_not_started()
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>()
                                                     .AddCommand<NamedInteractiveOnlyCommand>()
                                                     .UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new string[] { "named-interactive-only" }, isInputRedirected: false);

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("can be executed only in interactive mode, but this application is not running in this mode.");
        }

        [Fact]
        public async Task Application_can_be_created_and_executed_with_benchmark_commands()
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new string[] { "--str", "hello world", "-i", "-13", "-b" });

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().ContainEquivalentOf("{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
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
        public async Task Command_options_must_have_names_that_are_not_empty()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<EmptyOptionNameCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
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