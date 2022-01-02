namespace TypinExamples.Validation.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin;
    using TypinExamples.ExamplesTests.Common.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class PersonCommandTests
    {
        private readonly ITestOutputHelper _output;

        public PersonCommandTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("Arthur", 1)]
        [InlineData("Mike", 70)]
        [InlineData("Yu", 150)]
        public async Task ShouldRun(string name, int age)
        {
            //Arrange
            var builder = new CliApplicationBuilder()
                .UseStartup<Startup>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, $"person {name} {age}");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Trim().Should().Be($"{name} is {age} years old.");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("Eve", 0)]
        [InlineData("Mike", -85)]
        [InlineData("A", 1)]
        [InlineData("Arthur", 151)]
        public async Task ShouldShowError(string name, int age)
        {
            //Arrange
            var builder = new CliApplicationBuilder()
                .UseStartup<Startup>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, $"person {name} {age}");

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Trim().Should().NotBe($"{name} is {age} years old.");
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}
