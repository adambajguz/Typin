namespace Typin.Internal
{
    using System.Threading;

    internal sealed class CliContextAccessor : ICliContextAccessor
    {
        private readonly AsyncLocal<CliContextHolder?> _cliContextCurrent = new();

        /// <inheritdoc/>
        public CliContext? CliContext
        {
            get => _cliContextCurrent.Value?.Context;
            set
            {
                _cliContextCurrent.Value = new CliContextHolder { Context = value };
            }
        }

        private class CliContextHolder
        {
            public CliContext? Context;
        }
    }
}
