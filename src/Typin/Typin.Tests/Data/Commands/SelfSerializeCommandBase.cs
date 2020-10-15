namespace Typin.Tests.Data.Commands
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Typin.Console;

    public abstract class SelfSerializeCommandBase : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            string json = JsonConvert.SerializeObject(this);
            console.Output.WriteLine(json);

            return default;
        }
    }
}