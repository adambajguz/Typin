﻿namespace Typin.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Tests.Commands.Invalid;
    using Typin.Tests.Commands.Valid;
    using Xunit;
    using Xunit.Abstractions;

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
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(new VirtualConsole(Stream.Null))
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
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseInteractiveMode()
                .UsePromptForeground(ConsoleColor.Magenta)
                .UseConsole(new VirtualConsole(Stream.Null))
                .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
             app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                .AddCommands(new[] { typeof(DefaultCommand) })
                .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseInteractiveMode(false, false)
                .UsePromptForeground(ConsoleColor.Magenta)
                .UseConsole(new VirtualConsole(Stream.Null))
                .Build();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void Application_can_be_created_with_VirtualConsole_CreateBuffered()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            // Act
            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommandsFrom(typeof(DefaultCommand).Assembly)
                .AddCommands(new[] { typeof(DefaultCommand) })
                .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(console)
                .Build();

            // Assert
            app.Should().NotBeNull();
        }
    }
}