namespace Typin.Tests.Data.Commands
{
    using System.Threading.Tasks;
    using Typin.Console;
    using Typin.Tests.Extensions;

    public abstract class SelfSerializeCommandBase : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            string json = this.SerializeJson();
            console.Output.WriteLine(json);

            return default;
        }
    }
}