namespace Typin.Directives
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Directive("custom-throwable", Description = "Custom throwable directive.")]
    public sealed class CustomThrowableDirective : IDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirective);
        public const int ExpectedExitCode = 1;

        public bool ContinueExecution => true;

        public CustomThrowableDirective()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            console.Output.Write(ExpectedOutput);

            throw new DirectiveException();
        }
    }
}
