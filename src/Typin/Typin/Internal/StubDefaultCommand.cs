namespace Typin.Internal
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Internal.Schemas;
    using Typin.Schemas;

    [Command]
    internal class StubDefaultCommand : ICommand
    {
        public static CommandSchema Schema { get; } = CommandSchemaResolver.Resolve(typeof(StubDefaultCommand))!;

        public ValueTask ExecuteAsync(IConsole console)
        {
            return default;
        }
    }
}
