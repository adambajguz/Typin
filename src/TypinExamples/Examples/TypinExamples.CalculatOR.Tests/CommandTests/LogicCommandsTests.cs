namespace TypinExamples.CalculatOR.Tests.CommandTests.Arithmetic
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using TypinExamples.CalculatOR.Commands.Arithmetic;
    using TypinExamples.CalculatOR.Services;
    using TypinExamples.ExamplesTests.Common.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class LogicCommandsTests
    {
        private readonly ITestOutputHelper _output;

        public LogicCommandsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("and 1 1", "1")]
        [InlineData("and 1 1 1", "1")]
        [InlineData("and 1 1 0", "0")]
        [InlineData("and 0b1110 0b1011", "0b01010")]

        [InlineData("nand 1 1", "-2")]
        [InlineData("nand 1 1 1", "-1")]
        [InlineData("nand 1 1 0", "-1")]
        [InlineData("nand 0b1110 0b1011", "0b11110101")]

        [InlineData("nor 1 1", "-2")]
        [InlineData("nor 0 0", "-1")]
        [InlineData("nor 0 1", "-2")]
        [InlineData("nor 1 1 1", "0")]
        [InlineData("nor 1 1 0", "1")]
        [InlineData("nor 0b1110 0b1011", "0b11110000")]

        [InlineData("not 10", "-11")]
        [InlineData("not 0xF1", "0x0E")]
        [InlineData("not 0b10", "0b11111101")]
        [InlineData("not 0b0111", "0b11111000")]

        [InlineData("or 1 1", "1")]
        [InlineData("or 1 1 1", "1")]
        [InlineData("or 1 1 0", "1")]
        [InlineData("or 0b1110 0b1011", "0b01111")]

        [InlineData("lsh 1 -n 1", "2")]
        [InlineData("lsh 1 -n 2", "4")]
        [InlineData("lsh 11 -n 3", "88")]
        [InlineData("lsh 0b111 -n 5", "0b011100000")]

        [InlineData("rsh 1 -n 1", "0")]
        [InlineData("rsh 0b10 -n 1", "0b01")]
        [InlineData("rsh 1 -n 2", "0")]
        [InlineData("rsh 100 -n 3", "12")]
        [InlineData("rsh 0b111 -n 3", "0b0")]
        [InlineData("rsh 0b111000 -n 3", "0b0111")]

        [InlineData("xnor 1 1", "-1")]
        [InlineData("xnor 1 1 1", "1")]
        [InlineData("xnor 1 1 0", "0")]
        [InlineData("xnor 0b111 0b101", "0b11111101")]

        [InlineData("xor 1 1", "0")]
        [InlineData("xor 1 1 1", "1")]
        [InlineData("xor 1 1 0", "0")]
        [InlineData("xor 0b111 0b101", "0b010")]
        public async Task ShouldRun(string args, string result)
        {
            //Arrange
            var builder = new CliApplicationBuilder()
                .ConfigureServices((services) => services.AddSingleton<OperationEvaluatorService>())
                .AddCommandsFrom(typeof(AddCommand).Assembly);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, args);

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Trim().Should().Be(result);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}
