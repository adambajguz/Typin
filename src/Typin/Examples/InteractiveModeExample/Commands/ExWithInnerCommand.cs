namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("exi", Description = "Throws exception with inner exception that cannot be handled.")]
    public class ExWithInnerCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                Throw();
                return default;
            }
            catch (ApplicationException ex)
            {
                throw new OverflowException("Demo exception.", ex);
            }
        }

        public void Throw()
        {
            throw new ApplicationException("Demo inner exception");
        }
    }
}