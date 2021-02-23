namespace Typin.Tests.Dummy.Commands
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class HelloWorldCommand : ICommand
    {
        [CommandOption("target", FallbackVariableName = "ENV_TARGET")]
        public string Target { get; init; } = "World";

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine($"Hello {Target}!");

            return default;
        }
    }
}