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

    public class ArithmeticCommandsTests
    {
        private readonly ITestOutputHelper _output;

        public ArithmeticCommandsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("add 1 2", "3")]
        [InlineData("add 0x92 10", "0x9C")]
        [InlineData("add 0x91 0b10", "0x93")]
        [InlineData("add 0b11 0b10", "0b0101")]
        [InlineData("add 1 2 3 4 5", "15")]
        [InlineData("add 1 2 3 4 5 -b BIN", "0b01111")]

        [InlineData("subtract 10 2", "8")]
        [InlineData("subtract 0x92 10", "0x88")]
        [InlineData("subtract 0x91 0b10", "0x8F")]
        [InlineData("subtract 0b11 0b10", "0b01")]
        [InlineData("subtract 20 2 3 4 5", "6")]
        [InlineData("subtract 20 10 20", "-10")]
        [InlineData("subtract 1 2 3 4 5 -b BIN", "0b11110011")]

        [InlineData("multiply 10 2", "20")]
        [InlineData("multiply 0x92 10", "0xBB4")]
        [InlineData("multiply 0x91 0b10", "0xF22")]
        [InlineData("multiply 0b11 0b10", "0b0110")]
        [InlineData("multiply 20 2 3 4 5", "2400")]
        [InlineData("multiply 20 10 20", "4000")]
        [InlineData("multiply 1 2 3 4 5 -b BIN", "0b01111000")]

        [InlineData("divide 10 2", "5")]
        [InlineData("divide 0x92 10", "0xF5")]
        [InlineData("divide 0x91 0b10", "0xC9")]
        [InlineData("divide 0b11 0b10", "0b01")]
        [InlineData("divide 200 2 3 4", "8")]
        [InlineData("divide 20 10 20", "0")]
        [InlineData("divide 1000 2 3 4 5 -b BIN", "0b01000")]
        public async Task ShouldRun(string args, string result)
        {
            //Arrange
            var builder = new CliApplicationBuilder()
                .ConfigureServices((services) => services.AddSingleton<OperationEvaluatorService>())
                .AddCommandsFrom(typeof(AddCommand).Assembly);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, args);

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Trim().Should().Be(result);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}
