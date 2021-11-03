namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.DynamicCommands.Valid;
    using Typin.Tests.Data.Valid.Commands;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class DynamicCommandsApplicationTests
    {
        private readonly ITestOutputHelper _output;

        public DynamicCommandsApplicationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Application_cannot_be_created_with_a_dynamic_command()
        {
            // Act
            var builder = new CliApplicationBuilder()
                .AddCommand<AddValidDynamicAndExecuteCommand>()
                .AddDynamicCommand(typeof(ValidDynamicCommand))
                .UseDirectMode(true)
                .UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "add valid-dynamic-and-execute --name abc");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "Successfully added dynamic command 'abc'",
                @"{""Param0"":""test1"",""Opt0"":"""",""Opt1"":0,",
                @"""Param0"":""abc"",""Opt0"":"""",""Opt1"":0,",
                "test",
                "Parameter",
                "Number",
                "Param2",
                "Opt4",
                "Opt5",
                "Price"
            );
            stdErr.GetString().Should().BeEmpty();
        }
    }
}