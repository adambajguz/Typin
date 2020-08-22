namespace Typin.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Tests.Commands.Valid;
    using Typin.Tests.Internal;
    using Xunit;
    using Xunit.Abstractions;

    public class DependencyInjectionSpecs
    {
        private readonly ITestOutputHelper _output;

        public DependencyInjectionSpecs(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Delegate_type_activator_throws_if_the_underlying_function_returns_null()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<WithDependenciesCommand>()
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
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
            int exitCode = await app.RunAsync(new string[] { "--str", "hello world", "-i", "13", "-b" }, new Dictionary<string, string>());
            var commandInstance = stdOut.GetString().DeserializeJson<WithDependenciesCommand>();

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();

            var dependencyB = new DependencyB();
            commandInstance.Should().BeEquivalentTo(new WithDependenciesCommand(new DependencyA(),
                                                                                dependencyB,
                                                                                new DependencyC(dependencyB)));
        }
    }
}