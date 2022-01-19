namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Schemas.Attributes;

    [Alias("exi")]
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