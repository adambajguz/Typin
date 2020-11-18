//namespace Typin.InteractiveModeDemo.Commands
//{
//    using System.Threading.Tasks;
//    using Typin.Attributes;
//    using Typin.Console;

//    [Command("test", Description = "Prints a middleware pipeline structure in application.")]
//    public class TestCommand : ICommand
//    {
//        [CommandOption("x", 'a', IsRequired = true)]
//        public string Author { get; set; } = "";

//        [CommandOption('x', IsRequired = true)]
//        public string AuthorX { get; set; } = "";

//        public ValueTask ExecuteAsync(IConsole console)
//        {
//            console.Output.WriteLine("{Author} {AuthorX}");

//            return default;
//        }
//    }
//}
