namespace Typin.Internal
{
    using System.Threading;

    internal class CliContextAccessor : ICliContextAccessor
    {
        private readonly AsyncLocal<CliContextHolder> _cliContextCurrent = new();

        /// <inheritdoc/>
        public CliContext? CliContext
        {
            get => _cliContextCurrent.Value?.Context;
            set
            {
                CliContextHolder? holder = _cliContextCurrent.Value;
                if (holder is not null)
                {
                    // Clear current HttpContext trapped in the AsyncLocals, as its done.
                    holder.Context = null;
                }

                if (value is not null)
                {
                    // Use an object indirection to hold the HttpContext in the AsyncLocal,
                    // so it can be cleared in all ExecutionContexts when its cleared.
                    _cliContextCurrent.Value = new CliContextHolder { Context = value };
                }
            }
        }

        private class CliContextHolder
        {
            public CliContext? Context;
        }
    }
}
