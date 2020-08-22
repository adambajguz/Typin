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

    public class DirectivesSpecs
    {
        private readonly ITestOutputHelper _output;

        public DirectivesSpecs(ITestOutputHelper output)
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
    }
}