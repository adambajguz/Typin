namespace Typin.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.Services;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class DependencyInjectionTests
    {
        private readonly ITestOutputHelper _output;

        public DependencyInjectionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Should_scoped_services_be_resolved()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<WithDependenciesCommand>()
                .ConfigureServices(services =>
                {
                    services.AddTransient<DependencyA>();
                    services.AddScoped<DependencyB>();
                    services.AddTransient<DependencyC>();
                });

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new string[] { "cmd" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();

            string[] output = stdOut.GetString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
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
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<WithDependenciesCommand>()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<DependencyA>();
                    services.AddSingleton<DependencyB>();
                    services.AddSingleton<DependencyC>();
                });

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new string[] { "cmd" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();

            string[] output = stdOut.GetString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
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
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<WithDependenciesCommand>()
                .ConfigureServices(services =>
                {
                    services.AddTransient<DependencyA>();
                    services.AddTransient<DependencyB>();
                    services.AddTransient<DependencyC>();
                });

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new string[] { "cmd" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();

            string[] output = stdOut.GetString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
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