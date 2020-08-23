namespace Typin.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomDirectives.Valid;
    using Typin.Tests.Data.Middlewares;
    using Typin.Tests.Data.Startups;
    using Typin.Utilities.CliFx.Utilities;
    using Xunit;

    public class ApplicationBuilderTests
    {
        [Fact]
        public void Application_can_be_created_with_a_default_configuration()
        {
            // Act
            var app = new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_can_be_build_only_once()
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommandsFromThisAssembly();

            // Act
            Action act = () =>
            {
                builder.Build();
                builder.Build();
            };

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Application_in_normal_mode_can_be_created_with_a_custom_configuration()
        {
            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                .AddCommands(new[] { typeof(DefaultCommand) })
                .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomDirective>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(new VirtualConsole(Stream.Null))
                .UseStartupMessage("Startup message {{title}} {title} {version} {executable} {description}")
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_in_normal_mode_can_be_created_with_a_custom_configuration_and_middlewares()
        {
            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                .AddCommands(new[] { typeof(DefaultCommand) })
                .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective(typeof(CustomDirective))
                .UseMiddleware<ExecutionTimingMiddleware>()
                .UseMiddleware(typeof(ExitCodeMiddleware))
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(new VirtualConsole(Stream.Null))
                .UseStartupMessage("Startup message {{title}} {title} {version} {executable} {description}")
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_in_interactive_mode_can_be_created_with_a_custom_configuration()
        {
            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                .AddCommands(new[] { typeof(DefaultCommand) })
                .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .AddDirective<CustomDirective>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseInteractiveMode()
                .UsePromptForeground(ConsoleColor.Magenta)
                .UseStartupMessage("Startup message {{title}} {title} {version} {executable} {description}")
                .UseConsole(new VirtualConsole(Stream.Null))
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_in_interactive_mode_with_params_can_be_created_with_a_custom_configuration()
        {
            // Act
            var app = new CliApplicationBuilder()
               .AddCommand<DefaultCommand>()
               .AddCommandsFrom(typeof(DefaultCommand).Assembly)
               .AddCommands(new[] { typeof(DefaultCommand) })
               .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
               .AddCommandsFromThisAssembly()
               .AddDirective<DebugDirective>()
               .AddDirective<PreviewDirective>()
               .AddDirective<CustomInteractiveModeOnlyDirective>()
               .AddDirective<CustomDirective>()
               .UseTitle("test")
               .UseExecutableName("test")
               .UseVersionText("test")
               .UseDescription("test")
               .UseInteractiveMode(false, false)
               .UsePromptForeground(ConsoleColor.Magenta)
               .UseStartupMessage("Startup message {{title}} {title} {version} {executable} {description}")
               .UseConsole<SystemConsole>()
               .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_can_be_created_with_VirtualConsole_MemoryStreamWriter()
        {
            // Arrange
            IConsole console = new VirtualConsole(output: new MemoryStreamWriter(), isOutputRedirected: false,
                                                  error: new MemoryStreamWriter(Encoding.UTF8), isErrorRedirected: true);

            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                .AddCommands(new[] { typeof(DefaultCommand) })
                .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .AddDirective<CustomDirective>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(console)
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_can_be_created_with_VirtualConsole_CreateBuffered()
        {
            // Arrange
            var (console, _, _) = VirtualConsole.CreateBuffered();

            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                .AddCommands(new[] { typeof(DefaultCommand) })
                .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .AddDirective<CustomDirective>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(console)
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_can_be_created_with_startup_class()
        {
            // Act
            var app = new CliApplicationBuilder()
                .UseStartup<CustomStartupClass>()
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_can_be_created_with_configuration_action()
        {
            // Act
            var app = new CliApplicationBuilder()
                .Configure(cfg =>
                {
                    cfg.AddCommand<DefaultCommand>()
                       .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                       .AddCommands(new[] { typeof(DefaultCommand) })
                       .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                       .AddCommandsFromThisAssembly()
                       .AddDirective<DebugDirective>()
                       .AddDirective<PreviewDirective>()
                       .AddDirective<CustomInteractiveModeOnlyDirective>()
                       .AddDirective<CustomDirective>();
                })
                .Configure(cfg =>
                {
                    cfg.UseTitle("test")
                       .UseExecutableName("test")
                       .UseVersionText("test")
                       .UseDescription("test")
                       .UseConsole<SystemConsole>();
                })
                .Build();

            // Assert
            app.Should().NotBeNull();
        }
    }
}