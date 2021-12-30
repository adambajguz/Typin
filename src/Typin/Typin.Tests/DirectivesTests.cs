namespace Typin.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Modes;
    using Typin.Modes.Interactive;
    using Typin.Tests.Data.Common.Extensions;
    using Typin.Tests.Data.Invalid.CustomDirectives;
    using Typin.Tests.Data.Valid.Commands;
    using Typin.Tests.Data.Valid.CustomDirectives;
    using Typin.Tests.Data.Valid.DefaultCommands;
    using Typin.Utilities.Diagnostics.Directives;
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
        public async Task Custom_middleware_pipeline_should_throw_exception_after_detecting_middleware()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddDirective<CustomDirective>();
            //.UseMiddleware<DirectivesCheckMiddleware>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "[custom]");

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().Contain(CustomDirective.ExpectedOutput);
            stdErr.GetString().Should().Contain("System.ApplicationException: custom directive detected");
        }

        [Fact]
        public async Task Invalid_directive_type_should_throw_error()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddDirective(typeof(NamedCommand));

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().Be(ExitCode.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("not a valid directive type.");
        }

        [Fact]
        public async Task Direct_mode_application_cannot_process_interactive_directive()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddCommand<DefaultCommand>()
                .AddDirective<PreviewDirective>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "[interactive]" });

            // Assert
            exitCode.Should().Be(ExitCode.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("Unknown directive '[interactive]'.");
        }

        [Fact]
        public async Task Preview_directive_can_be_specified_to_print_provided_arguments_as_they_were_parsed()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<PreviewDirective>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[preview]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdOut.GetString().Should().ContainAll(
                "named", "<param>", "(-a)", "(-b)", "(-c)", "(--option \"foo\")"
            );
        }

        [Fact]
        public async Task Preview_directive_can_be_specified_before_debug_directive()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>()
                .AddDirective<PreviewDirective>()
                .AddDirective<DebugDirective>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[preview]", "[debug]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdOut.GetString().Should().NotContain("Attach debugger to PID");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
            stdOut.GetString().Should().ContainAll(
                "[preview] [debug]", "named", "<param>", "(-a)", "(-b)", "(-c)", "(--option \"foo\")"
            );
        }

        [Fact]
        public async Task Preview_directive_can_be_specified_after_debug_directive()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>()
                .AddDirective<PreviewDirective>()
                .AddDirective<FakeDebugDirective>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[debug]", "[preview]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdOut.GetString().Should().Contain("Attach debugger to PID");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
            stdOut.GetString().Should().ContainAll(
                "[debug] [preview]", "named", "<param>", "(-a)", "(-b)", "(-c)", "(--option \"foo\")"
            );
        }

        [Fact]
        public async Task Preview_directive_should_work_in_direct_mode_even_if_directives_from_other_mode_are_specified()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>()
                .AddDirective<PreviewDirective>()
                .AddDirective<DebugDirective>(); //TODO: add test when UseInteractive and AddDirective<ScopeUp> are used and check if error is thrown
            //TODO: what if unknown directive is passed after [preview]? app wil throw ex => maybe some propery in app config to allow relaxed directive handling?

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[preview]", "[debug]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdOut.GetString().Should().NotContain("Attach debugger to PID");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Custom_stop_directive_should_cancel_execution_after_running()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>()
                .AddDirective<PreviewDirective>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[custom-stop]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(CustomStopDirective.ExpectedExitCode);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdOut.GetString().Should().Be(CustomStopDirective.ExpectedOutput);
        }

        [Fact]
        public async Task Custom_directive_should_run()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "[custom]", "named" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdOut.GetString().Should().ContainAll(
                CustomDirective.ExpectedOutput, NamedCommand.ExpectedOutputText
            );
        }

        //[Fact]
        //public async Task Default_directive_should_not_allow_default_command_to_execute_when_there_is_a_name_conflict()
        //{
        //    // Arrange
        //    var builder = new CliApplicationBuilder()
        //         .AddCommand<DefaultCommandWithParameter>()
        //         .AddCommand<NamedCommand>()
        //         .AddDirective<DefaultDirective>();

        //    // Act
        //    var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "[!]", "named" });

        //    // Assert
        //    exitCode.Should().Be(ExitCode.Success);
        //    stdOut.GetString().Should().NotBeNullOrWhiteSpace();
        //    stdOut.GetString().Should().Contain(NamedCommand.ExpectedOutputText);
        //    stdOut.GetString().Should().NotContainAll(
        //        "named", DefaultCommandWithParameter.ExpectedOutputText
        //    );

        //    _output.WriteLine(stdOut.GetString());
        //}

        [Fact]
        public async Task Custom_interactive_directive_should_not_run_in_direct_mode()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[custom-interactive]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdOut.GetString().Should().NotContainAll(
                "@ [custom-interactive]", "Description", "Usage", "Directives", "[custom]"
            );
            stdErr.GetString().Should().Contain($"Directive '{typeof(CustomInteractiveModeOnlyDirective).FullName}' contains invalid supported mode(s)");
        }

        [Fact]
        public async Task Custom_throwable_directive_should_throw_exception()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomThrowableDirective>()
                .AddDirective<CustomThrowableDirectiveWithMessage>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[custom-throwable]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(CustomThrowableDirective.ExpectedExitCode);
            stdOut.GetString().Should().Be(CustomThrowableDirective.ExpectedOutput);
            stdErr.GetString().Should().ContainEquivalentOf(
                "An unhandled error occured while executing pipeline"
            );
        }

        [Fact]
        public async Task Custom_throwable_directive_with_message_should_throw_exception()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomThrowableDirective>()
                .AddDirective<CustomThrowableDirectiveWithMessage>()
                .AddDirective<CustomDirective>()
                .AddDirective<CustomStopDirective>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[custom-throwable-with-message]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(CustomThrowableDirectiveWithMessage.ExpectedExitCode);
            stdOut.GetString().Should().Be(CustomThrowableDirectiveWithMessage.ExpectedOutput);
            stdErr.GetString().Should().ContainEquivalentOf(CustomThrowableDirectiveWithMessage.ExpectedExceptionMessage);
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
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective(typeof(NoInterafaceDirective));

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[invalid-no-interface]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(ExitCode.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Custom_directive_should_have_proper_attribute()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<NoAttributeDirective>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(ExitCode.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }


        [Fact]
        public async Task Custom_directive_should_be_registered_once()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<PreviewDirective>()
                .AddDirective<DuplicatedDirective>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[preview]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(ExitCode.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("[preview]");
        }

        [Fact]
        public async Task Custom_directive_should_have_non_empty_name()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<PreviewDirective>()
                .AddDirective<EmptyNameDirective>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[preview]", "named", "param", "-abc", "--option", "foo" },
                new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(ExitCode.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("[  ]");
        }

        [Fact]
        public async Task Custom_directive_should_be_registered_to_be_used()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[preview]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(ExitCode.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("Unknown directive '[preview]'.");
        }

        [Fact]
        public async Task Interactive_only_directive_cannot_be_executed_in_direct_mode()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                new[] { "[custom-interactive]", "named", "param", "-abc", "--option", "foo" });

            // Assert
            exitCode.Should().Be(ExitCode.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdOut.GetString().Should().NotContainAll("-h", "--help");
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().ContainAll($"This application is running in '{typeof(DirectMode).FullName}' mode.",
                                                   $"Directive '{typeof(CustomInteractiveModeOnlyDirective).FullName}'");
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
        //    exitCode.Should().Be(ExitCode.Success);
        //    stdOut.GetString().Should().NotContainAll(
        //        "@ [custom-interactive]", "Description", "Usage", "Directives", "[custom]"
        //    );
        //    stdOut.GetString().Should().Contain(CustomInteractiveModeOnlyDirective.ExpectedOutput);

        //    stdErr.GetString().Should().NotContainAll(
        //        "Directive", "[custom-interactive]", "is for interactive mode only."
        //    );
        //}
    }
}