namespace Typin.Internal
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Internal.Schemas;
    using Typin.Schemas;

    [Command]
    internal class StubDefaultCommand : ICommand
    {
        public static CommandSchema Schema { get; } = CommandSchemaResolver.Resolve(typeof(StubDefaultCommand), null);

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
