namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Modes;
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
        [InlineData(new string[] { "--str", "hello world", "-i", "13", "-b" }, "{\"StrOption\":\"hello world\",\"IntOption\":13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello world", "-i", "13", "-b" }, "{\"StrOption\":\"hello world\",\"IntOption\":13,\"BoolOption\":true}", true)]
        [InlineData(new string[] { "--str", "hello world", "-i", "-13", "-b" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello world", "-i", "-13", "-b" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true)]
        [InlineData(new string[] { "--str", "hello world", "-i", "-13", "-b", "" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello world", "-i", "-13", "-b", "" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true)]
        [InlineData(new string[] { "--str", "hello world", "", "-i", "-13", "-b", "" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello world", "", "-i", "-13", "-b", "" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true)]
        [InlineData(new string[] { "" }, "{\"StrOption\":null,\"IntOption\":0,\"BoolOption\":false}", false)]
        [InlineData(new string[] { "" }, "{\"StrOption\":null,\"IntOption\":0,\"BoolOption\":false}", true)]
        public async Task Application_can_be_created_and_executed_with_list_command(string[] commandLineArguments, string result, bool interactive)
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>();

            if (interactive)
            {
                builder.UseDirectMode(true)
                       .UseInteractiveMode();
            }

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, commandLineArguments, interactive);

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().ContainEquivalentOf(result);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("--str \"hello \\ world\" -i 13 -b", "{\"StrOption\":\"hello \\\\ world\",\"IntOption\":13,\"BoolOption\":true}", false, false)]
        [InlineData("--str \"hello \\ world\" -i 13 -b", "{\"StrOption\":\"hello \\\\ world\",\"IntOption\":13,\"BoolOption\":true}", true, false)]
        [InlineData("--str \"hello \\\" world\" -i 13 -b", "{\"StrOption\":\"hello \\\" world\",\"IntOption\":13,\"BoolOption\":true}", false, false)]
        [InlineData("--str \"hello \\\" world\" -i 13 -b", "{\"StrOption\":\"hello \\\" world\",\"IntOption\":13,\"BoolOption\":true}", true, false)]
        [InlineData("--str \"hello world\" -i 13 -b", "{\"StrOption\":\"hello world\",\"IntOption\":13,\"BoolOption\":true}", false, false)]
        [InlineData("", "{\"StrOption\":null,\"IntOption\":0,\"BoolOption\":false}", false, false)]
        [InlineData("test.dll --str \"hello world\" -i 13 -b", "{\"StrOption\":\"hello world\",\"IntOption\":13,\"BoolOption\":true}", true, true)]
        [InlineData("test.dll --str \"hello world\" -i -13 -b", "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false, true)]
        [InlineData("test.dll --str \"hello world\" -i -13 -b", "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true, true)]
        [InlineData("test.dll --str \"hello world\" -i \"-13\" \"-b\"", "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false, true)]
        [InlineData("test.dll --str \"hello world\" -i \"-13\" \"-b\"", "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true, true)]
        [InlineData("test.dll", "{\"StrOption\":null,\"IntOption\":0,\"BoolOption\":false}", false, true)]
        [InlineData("test.dll", "{\"StrOption\":null,\"IntOption\":0,\"BoolOption\":false}", true, true)]
        public async Task Application_can_be_created_and_executed_with_string_command(string commandLine, string result, bool interactive, bool containsExecutable)
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>();

            if (interactive)
            {
                builder.UseInteractiveMode();
            }

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, commandLine, containsExecutable, isInputRedirected: interactive);

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
            stdErr.GetString().Should().Contain($"Command '{typeof(NamedInteractiveOnlyCommand).FullName}' contains an invalid mode in SupportedModes parameter.");
        }

        [Fact]
        public async Task Application_without_interactive_mode_cannot_execute_interactive_only_commands_even_if_supports_interactive_mode_but_is_not_started()
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>()
                                                     .AddCommand<NamedInteractiveOnlyCommand>()
                                                     .UseDirectMode(true)
                                                     .UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new string[] { "named-interactive-only" }, isInputRedirected: false);

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain($"This application is running in '{typeof(DirectMode).FullName}' mode.");
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