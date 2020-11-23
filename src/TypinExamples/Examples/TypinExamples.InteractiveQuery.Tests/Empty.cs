namespace TypinExamples.InterctiveQuery.Tests
{
    using FluentAssertions;
    using Xunit;
    using Xunit.Abstractions;

    public class Empty
    {
        private readonly ITestOutputHelper _output;

        public Empty(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ShouldRun()
        {
            true.Should().Be(true);
        }
    }
}
