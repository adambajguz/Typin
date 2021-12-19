﻿namespace Typin.Tests
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Exceptions;
    using Typin.Modes;
    using Typin.OptionFallback;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomDirectives.Valid;
    using Typin.Tests.Data.Middlewares;
    using Typin.Tests.Data.Modes.Valid;
    using Typin.Tests.Data.Startups;
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
        public void Application_in_direct_mode_can_be_created_with_a_custom_configuration()
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
                .UseOptionFallbackProvider<EnvironmentVariableFallbackProvider>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(new VirtualConsole(Stream.Null))
                .UseStartupMessage($"Startup message")
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_in_direct_mode_can_be_created_with_a_custom_configuration_and_middlewares()
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
                .UseOptionFallbackProvider(typeof(EnvironmentVariableFallbackProvider))
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(new VirtualConsole(Stream.Null))
                .UseStartupMessage((metadata) => $"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description}")
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
                .AddDirectivesFrom(typeof(DefaultCommand).Assembly)
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseDirectMode(true)
                .UseInteractiveMode()
                .RegisterMode<ValidCustomMode>()
                .RegisterMode(typeof(ValidCustomMode))
                .UseInteractiveMode()
                .UseStartupMessage((metadata, console) => { console.Output.WriteLine($"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description})"); })
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
               .AddDirectivesFromThisAssembly()
               .UseTitle("test")
               .UseExecutableName("test")
               .UseVersionText("test")
               .UseDescription("test")
               .UseInteractiveMode()
               .UseStartupMessage((metadata) => $"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description}")
               .UseConsole<SystemConsole>()
               .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_can_be_created_with_VirtualConsole_MemoryStreamWriter()
        {
            // Arrange
            var (console, _, _) = VirtualConsole.CreateBuffered(isInputRedirected: false, isOutputRedirected: true);

            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                .AddCommands(new[] { typeof(DefaultCommand) })
                .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddCommandsFromThisAssembly()
                .AddDirectivesFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .AddDirective<CustomDirective>()
                .UseConsole(console)
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_can_be_created_with_VirtualConsole_CreateBuffered()
        {
            // Arrange
            var (console, _, _) = VirtualConsole.CreateBuffered(isInputRedirected: false);

            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                .AddCommands(new[] { typeof(DefaultCommand) })
                .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddCommandsFromThisAssembly()
                .UseExceptionHandler(typeof(DefaultExceptionHandler))
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
                       .UseExceptionHandler<DefaultExceptionHandler>()
                       .AddDirective<DebugDirective>()
                       .AddDirective<PreviewDirective>()
                       .AddDirective<CustomInteractiveModeOnlyDirective>()
                       .AddDirective<CustomDirective>();

                    cfg.UseStartupMessage((metadata) => $"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description}");
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