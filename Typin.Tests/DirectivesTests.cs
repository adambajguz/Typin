namespace Typin.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Tests.Commands.Valid;
    using Xunit;
    using Xunit.Abstractions;

    public class DirectivesTests
    {
        private readonly ITestOutputHelper _output;

        public DirectivesTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Preview_directive_can_be_specified_to_print_provided_arguments_as_they_were_parsed()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<PreviewDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[preview]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().ContainAll(
                "named", "<param>", "[-a]", "[-b]", "[-c]", "[--option \"foo\"]"
            );

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Custom_stop_directive_should_cancel_execution_after_running()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>()
                .AddDirective<PreviewDirective>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[custom-stop]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().Be(CustomStopDirective.ExpectedOutput);
        }

        [Fact]
        public async Task Custom_directive_should_run()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[custom]", "named" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().ContainAll(
                CustomDirective.ExpectedOutput, NamedCommand.ExpectedOutputText
            );

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Custom_interactive_directive_should_not_run_in_normal_mode()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[custom-interactive]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().NotBe(0);
            stdOut.GetString().Should().ContainAll(
                "@ [custom-interactive]", "Description", "Usage", "Directives", "[custom]"
            );
            stdErr.GetString().Should().ContainAll(
                "Directive", "[custom-interactive]", "is for interactive mode only."
            );
            _output.WriteLine(stdOut.GetString());
        }

        //[Fact]
        //public async Task Custom_interactive_directive_should_run_in_interactive_mode()
        //{
        //    // Arrange
        //    var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

        //    var application = new CliApplicationBuilder()
        //        .AddCommand<NamedCommand>()
        //        .UseConsole(console)
        //        .AddDirective<PreviewDirective>()
        //        .AddDirective<CustomDirective>()
        //        .AddDirective<CustomStopDirective>()
        //        .AddDirective<CustomInteractiveModeOnlyDirective>()
        //        .UseInteractiveMode()
        //        .Build();

        //    // Act
        //    int exitCode = await application.RunAsync(
        //        new[] { "[interactive]", "[custom-interactive]", "named", Environment.NewLine },
        //        new Dictionary<string, string>());

        //    // Assert
        //    exitCode.Should().Be(0);
        //    stdOut.GetString().Should().NotContainAll(
        //        "@ [custom-interactive]", "Description", "Usage", "Directives", "[custom]"
        //    );
        //    stdOut.GetString().Should().Contain(CustomInteractiveModeOnlyDirective.ExpectedOutput);

        //    stdErr.GetString().Should().NotContainAll(
        //        "Directive", "[custom-interactive]", "is for interactive mode only."
        //    );
        //    _output.WriteLine(stdOut.GetString());
        //}
    }
}