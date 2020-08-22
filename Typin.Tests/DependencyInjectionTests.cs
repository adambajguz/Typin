namespace Typin.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Tests.Commands.Valid;
    using Typin.Tests.Internal;
    using Xunit;
    using Xunit.Abstractions;

    public class DependencyInjectionTests
    {
        [Fact]
        public async Task Should_scoped_services_be_resolved()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<WithDependenciesCommand>()
                .UseConsole(console)
                .ConfigureServices(services =>
                {
                    services.AddTransient<DependencyA>();
                    services.AddScoped<DependencyB>();
                    services.AddTransient<DependencyC>();
                })
                .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { "cmd" }, new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();

            string[] output = stdOut.GetString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            output.Should().HaveCount(3);

            char[] chars = output[0].Split('|')
                                    .Select(x => char.Parse(x))
                                    .ToArray();

            Guid[] guids = output[1].Split('|')
                                    .Select(x => Guid.Parse(x))
                                    .ToArray();

            Guid depGuid = Guid.Parse(output[2]);

            chars.Should().HaveCount(3);
            chars.Should().Equal(new char[] { 'A', 'B', 'B' });

            guids.Should().HaveCount(3);
            depGuid.Should().Be(guids[1]);
        }

        [Fact]
        public async Task Should_singleton_services_be_resolved()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<WithDependenciesCommand>()
                .UseConsole(console)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<DependencyA>();
                    services.AddSingleton<DependencyB>();
                    services.AddSingleton<DependencyC>();
                })
                .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { "cmd" }, new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();

            string[] output = stdOut.GetString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            output.Should().HaveCount(3);

            char[] chars = output[0].Split('|')
                                    .Select(x => char.Parse(x))
                                    .ToArray();

            Guid[] guids = output[1].Split('|')
                                    .Select(x => Guid.Parse(x))
                                    .ToArray();

            Guid depGuid = Guid.Parse(output[2]);

            chars.Should().HaveCount(3);
            chars.Should().Equal(new char[] { 'A', 'B', 'B' });

            guids.Should().HaveCount(3);
            depGuid.Should().Be(guids[1]);
        }

        [Fact]
        public async Task Should_transient_services_be_resolved()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<WithDependenciesCommand>()
                .UseConsole(console)
                .ConfigureServices(services =>
                {
                    services.AddTransient<DependencyA>();
                    services.AddTransient<DependencyB>();
                    services.AddTransient<DependencyC>();
                })
                .Build();

            // Assert
            app.Should().NotBeNull();

            // Act
            int exitCode = await app.RunAsync(new string[] { "cmd" }, new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();

            string[] output = stdOut.GetString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            output.Should().HaveCount(3);

            char[] chars = output[0].Split('|')
                                    .Select(x => char.Parse(x))
                                    .ToArray();

            Guid[] guids = output[1].Split('|')
                                    .Select(x => Guid.Parse(x))
                                    .ToArray();

            Guid depGuid = Guid.Parse(output[2]);

            chars.Should().HaveCount(3);
            chars.Should().Equal(new char[] { 'A', 'B', 'B' });

            guids.Should().HaveCount(3);
            depGuid.Should().NotBe(guids[1]);
        }
    }
}