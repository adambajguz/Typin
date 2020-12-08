namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Modes;
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
        [InlineData(new string[] { "--str", "", "-i", "-13", "-b" }, "{\"StrOption\":\"\",\"IntOption\":-13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "", "-i", "-13", "-b" }, "{\"StrOption\":\"\",\"IntOption\":-13,\"BoolOption\":true}", true)]
        [InlineData(new string[] { "--str", "hello world", "-i", "-13", "-b", "" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello world", "-i", "-13", "-b", "" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true)]
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
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainEquivalentOf(result);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData(new string[] { "--str", "hello world", "", "-i", "-13", "-b", "" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", false)]
        [InlineData(new string[] { "--str", "hello world", "", "-i", "-13", "-b", "" }, "{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}", true)]
        [InlineData(new string[] { "" }, "{\"StrOption\":null,\"IntOption\":0,\"BoolOption\":false}", false)]
        [InlineData(new string[] { "" }, "{\"StrOption\":null,\"IntOption\":0,\"BoolOption\":false}", true)]
        public async Task Application_can_be_created_but_not_executed_with_list_command(string[] commandLineArguments, string result, bool interactive)
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
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().NotContainEquivalentOf(result);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
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
            exitCode.Should().Be(ExitCodes.Success);
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
            stdErr.GetString().Should().Contain($"Command '{typeof(NamedInteractiveOnlyCommand).FullName}' contains invalid supported mode(s)");
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
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainEquivalentOf("{\"StrOption\":\"hello world\",\"IntOption\":-13,\"BoolOption\":true}");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}