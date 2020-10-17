namespace Typin.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Tests.Data.Commands.Invalid;
    using Typin.Tests.Data.Commands.Valid;
    using Xunit;
    using Xunit.Abstractions;

    public class ApplicationTests
    {
        private readonly ITestOutputHelper _output;

        public ApplicationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Application_can_be_created_and_executed_with_benchmark_default_command()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            // Act
            var app = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>()
                                                 .UseConsole(console)
                                                 .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { "--str", "hello world", "-i", "13", "-b" }, new Dictionary<string, string>());

            // Asert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().ContainEquivalentOf("{\"StrOption\":\"hello world\",\"IntOption\":13,\"BoolOption\":true}");
            stdErr.GetString().Should().BeNullOrWhiteSpace();

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Application_with_interactive_mode_can_be_created_and_executed_in_normal_mode_with_benchmark_default_command()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            // Act
            var app = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>()
                                                 .UseInteractiveMode()
                                                 .UseConsole(console)
                                                 .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { "--str", "hello world", "-i", "-13", "-b" }, new Dictionary<string, string>());

            // Asert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().ContainEquivalentOf("{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}");
            stdErr.GetString().Should().BeNullOrWhiteSpace();

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Application_without_interactive_mode_cannot_execute_interactive_only_commands()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            // Act
            var app = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>()
                                                 .AddCommand<NamedInteractiveOnlyCommand>()
                                                 .UseConsole(console)
                                                 .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { "named-interactive-only" }, new Dictionary<string, string>());

            // Asert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("can be executed only in interactive mode, but this application is using CliApplication.");

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Application_without_interactive_mode_cannot_execute_interactive_only_commands_even_if_supports_interactive_mode_but_is_not_started()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            // Act
            var app = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>()
                                                 .AddCommand<NamedInteractiveOnlyCommand>()
                                                 .UseConsole(console)
                                                 .UseInteractiveMode()
                                                 .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { "named-interactive-only" }, new Dictionary<string, string>());

            // Asert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("can be executed only in interactive mode, but this application is not running in this mode.");

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Application_can_be_created_and_executed_with_benchmark_commands()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            // Act
            var app = new CliApplicationBuilder().AddCommand<BenchmarkDefaultCommand>()
                                                 .UseConsole(console)
                                                 .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { "--str", "hello world", "-i", "-13", "-b" }, new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().ContainEquivalentOf("{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task At_least_one_command_must_be_defined_in_an_application()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .UseConsole(console)
                .UseStartupMessage("{title} CLI {version} {{title}} {executable} {{{description}}} {test}")
                .Build();

            // Act
            int exitCode = await application.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Commands_must_implement_the_corresponding_interface()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand(typeof(NonImplementedCommand))
                .UseStartupMessage("{title} CLI {version} {{title}} {executable} {{{description}}} {test}")
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Commands_must_be_annotated_by_an_attribute()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NonAnnotatedCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Commands_must_have_unique_names()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<GenericExceptionCommand>()
                .AddCommand<CommandExceptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_can_be_default_but_only_if_it_is_the_only_such_command()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<OtherDefaultCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_option_must_have_valid_names()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<InvalidOptionNameCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_option_must_have_valid_shortnames()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<InvalidOptionShortNameCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_parameters_must_have_unique_order()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DuplicateParameterOrderCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_parameters_must_have_unique_names()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DuplicateParameterNameCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_parameter_can_be_non_scalar_only_if_no_other_such_parameter_is_present()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<MultipleNonScalarParametersCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_parameter_can_be_non_scalar_only_if_it_is_the_last_in_order()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<NonLastNonScalarParameterCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_options_must_have_names_that_are_not_empty()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<EmptyOptionNameCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_options_must_have_names_that_are_longer_than_one_character()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<SingleCharacterOptionNameCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_options_must_have_unique_names()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DuplicateOptionNamesCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_options_must_have_unique_short_names()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DuplicateOptionShortNamesCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_options_must_have_unique_environment_variable_names()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DuplicateOptionEnvironmentVariableNamesCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_options_must_not_have_conflicts_with_the_implicit_help_option()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<ConflictWithHelpOptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Command_options_must_not_have_conflicts_with_the_implicit_version_option()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<ConflictWithVersionOptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>());

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }
    }
}