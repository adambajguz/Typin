//namespace Typin.Tests
//{
//    using System.Threading.Tasks;
//    using FluentAssertions;
//    using Typin.Tests.Data.Commands.Valid;
//    using Typin.Tests.Data.Middlewares;
//    using Typin.Tests.Extensions;
//    using Xunit;
//    using Xunit.Abstractions;

//    public class MiddlewareTests
//    {
//        private readonly ITestOutputHelper _output;

//        public MiddlewareTests(ITestOutputHelper output)
//        {
//            _output = output;
//        }

//        [Fact]
//        public async Task Middleware_types_collection_should_contain_all_user_defined_middlewares()
//        {
//            // Arrange
//            var builder = new CliApplicationBuilder()
//                .AddCommand<DefaultCommand>()
//                .AddCommand<PipelineCommand>();
//            //.UseMiddleware<ExecutionTimingMiddleware>()
//            //.UseMiddleware(typeof(ExitCodeMiddleware));

//            // Act
//            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new string[] { "pipeline" });

//            // Assert
//            exitCode.Should().Be(ExitCode.Success);
//            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
//            stdErr.GetString().Should().BeNullOrWhiteSpace();

//            stdOut.GetString().Should().ContainAll(
//                typeof(ExecutionTimingMiddleware).AssemblyQualifiedName,
//                typeof(ExitCodeMiddleware).AssemblyQualifiedName,
//                PipelineCommand.PipelineTermination,
//                "Typin.Internal.Pipeline");
//        }

//        [Fact]
//        public async Task Middleware_pipeline_should_be_executed()
//        {
//            // Arrange
//            var builder = new CliApplicationBuilder()
//                .AddCommand<DefaultCommand>()
//                .AddCommand<PipelineCommand>();
//            //.UseMiddleware<ExecutionTimingMiddleware>()
//            //.UseMiddleware(typeof(ExitCodeMiddleware));

//            // Act
//            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

//            // Assert
//            exitCode.Should().Be(ExitCode.Success);
//            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
//            stdErr.GetString().Should().BeNullOrWhiteSpace();

//            stdOut.GetString().Should().ContainAll(
//                ExecutionTimingMiddleware.ExpectedOutput0,
//                ExecutionTimingMiddleware.ExpectedOutput1,
//                ExitCodeMiddleware.ExpectedOutput,
//                DefaultCommand.ExpectedOutputText);
//        }
//    }
//}