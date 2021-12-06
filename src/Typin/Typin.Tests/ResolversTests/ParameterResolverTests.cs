namespace Typin.Tests.ResolversTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Invalid;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class ParameterResolverTests
    {
        private readonly ITestOutputHelper _output;

        public ParameterResolverTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Command_parameters_must_have_unique_order()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateParameterOrderCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_parameters_must_have_unique_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DuplicateParameterNameCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_parameter_can_be_non_scalar_only_if_no_other_such_parameter_is_present()
        {
            var builder = new CliApplicationBuilder()
                .AddCommand<MultipleNonScalarParametersCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Command_parameter_can_be_non_scalar_only_if_it_is_the_last_in_order()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NonLastNonScalarParameterCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}