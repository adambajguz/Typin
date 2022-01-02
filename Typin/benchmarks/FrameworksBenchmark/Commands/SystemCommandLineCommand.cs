namespace FrameworksBenchmark.Commands
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Reflection;
    using System.Threading.Tasks;

    public class SystemCommandLineCommand
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public static int ExecuteHandler(string s, int i, bool b)
        {
            return 0;
        }
#pragma warning restore IDE0060 // Remove unused parameter

        public Task<int> ExecuteAsync(string[] args)
        {
            RootCommand command = new()
            {
                new Option(new[] { "--str", "-s" })
                {
                    Argument = new Argument<string>()
                },
                new Option(new[] { "--int", "-i" })
                {
                    Argument = new Argument<int>()
                },
                new Option(new[] { "--bool", "-b" })
                {
                    Argument = new Argument<bool>()
                }
            };

            MethodInfo? method = typeof(SystemCommandLineCommand).GetMethod(nameof(ExecuteHandler));
            command.Handler = CommandHandler.Create(method!);

            return command.InvokeAsync(args);
        }
    }
}