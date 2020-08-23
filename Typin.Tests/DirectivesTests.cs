namespace Typin.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomDirectives.Invalid;
    using Typin.Tests.Data.CustomDirectives.Valid;
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

        [Fact]
        public async Task Custom_throwable_directive_should_throw_exception()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomThrowableDirective>()
                .AddDirective<CustomThrowableDirectiveWithMessage>()
                .AddDirective<CustomThrowableDirectiveWithInnerException>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[custom-throwable]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(CustomThrowableDirective.ExpectedExitCode);
            stdOut.GetString().Should().Be(CustomThrowableDirective.ExpectedOutput);
            stdErr.GetString().Should().ContainEquivalentOf(
                "Typin.Exceptions.DirectiveException: Exception of type 'Typin.Exceptions.DirectiveException' was thrown."
            );
            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Custom_throwable_directive_with_message_should_throw_exception()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomThrowableDirective>()
                .AddDirective<CustomThrowableDirectiveWithMessage>()
                .AddDirective<CustomThrowableDirectiveWithInnerException>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[custom-throwable-with-message]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(CustomThrowableDirectiveWithMessage.ExpectedExitCode);
            stdOut.GetString().Should().Be(CustomThrowableDirectiveWithMessage.ExpectedOutput);
            stdErr.GetString().Should().ContainEquivalentOf(CustomThrowableDirectiveWithMessage.ExpectedExceptionMessage);

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Custom_throwable_directive_with_inner_exception_should_throw_exception()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomThrowableDirective>()
                .AddDirective<CustomThrowableDirectiveWithMessage>()
                .AddDirective<CustomThrowableDirectiveWithInnerException>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[custom-throwable-with-inner-exception]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(CustomThrowableDirectiveWithInnerException.ExpectedExitCode);
            stdOut.GetString().Should().Be(CustomThrowableDirectiveWithInnerException.ExpectedOutput);
            stdErr.GetString().Should().ContainEquivalentOf(CustomThrowableDirectiveWithInnerException.ExpectedExceptionMessage);

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public void Custom_directive_should_not_be_abstract()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            // Act & Assert
            Func<CliApplication> act = () =>
            {
                return new CliApplicationBuilder()
                    .AddCommand<NamedCommand>()
                    .UseConsole(console)
                    .AddDirective<AbstractDirective>()
                    .Build();
            };

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async Task Custom_directive_should_have_proper_interface()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective(typeof(NoInterafaceDirective))
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[invalid-no-interface]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Custom_directive_should_have_proper_attribute()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<NoAttributeDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }


        [Fact]
        public async Task Custom_directive_should_be_registered_once()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<PreviewDirective>()
                .AddDirective<DuplicatedDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[preview]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("[preview]");

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Custom_directive_should_have_non_empty_name()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<PreviewDirective>()
                .AddDirective<EmptyNameDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[preview]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("[  ]");

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Custom_directive_should_be_registered_to_be_used()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[preview]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("Unknown directive '[preview]'.");

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Interactive_only_directive_cannot_be_executed_in_normal_mode()
        {
            // Arrange
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var application = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .UseConsole(console)
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .Build();

            // Act
            int exitCode = await application.RunAsync(
                new[] { "[custom-interactive]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace(); //help
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("Directive '[custom-interactive]' is for interactive mode only. Thus, cannot be used in normal mode.");

            _output.WriteLine(stdOut.GetString());
            _output.WriteLine(stdErr.GetString());
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