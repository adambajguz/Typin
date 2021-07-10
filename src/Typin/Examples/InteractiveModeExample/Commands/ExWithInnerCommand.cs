namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;

    [Command("exi", Description = "Throws exception with inner exception that cannot be handled.")]
    public class ExWithInnerCommand : ICommand
    {
        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
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

        public static void Throw()
        {
            throw new ApplicationException("Demo inner exception");
        }
    }
}