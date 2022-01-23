namespace Typin.Modes.Programmatic
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Modes;

    /// <summary>
    /// Programmatic CLI mode to run commands programmatically with advanced options.
    /// </summary>
    public class ProgrammaticMode : ICliMode
    {
        private readonly ConcurrentQueue<IEnumerable<string>> _queue = new();

        private readonly IConsole _console;
        private readonly ICommandExecutor _commandExecutor;

        /// <summary>
        /// Shared input options.
        /// </summary>
        public InputOptions InputOptions { get; set; }

        /// <summary>
        /// Shared command execution options.
        /// </summary>
        public ModeBehavior Behavior { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="ProgrammaticMode"/>.
        /// </summary>
        public ProgrammaticMode(IConsole console,
                                ICommandExecutor commandExecutor)
        {
            _console = console;
            _commandExecutor = commandExecutor;
        }

        /// <summary>
        /// Queues a commands to execute.
        /// </summary>
        /// <param name="arguments"></param>
        public void Queue(IEnumerable<string> arguments)
        {
            _queue.Enqueue(arguments);
        }

        /// <summary>
        /// Queues a commands to execute.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="repeat"></param>
        public void Queue(IEnumerable<string> arguments, int repeat)
        {
            for (int i = 0; i < repeat; i++)
            {
                _queue.Enqueue(arguments);
            }
        }

        /// <summary>
        /// Gets queued commands.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string>[] GetQueued()
        {
            return _queue.ToArray();
        }

        /// <summary>
        /// Clears queued commands.
        /// </summary>
        /// <returns></returns>
        public void ClearQueued()
        {
            _queue.Clear();
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteAsync(CancellationToken cancellationToken) //TODO: maybe replace Task<int> with Task
        {
            while (!_queue.IsEmpty)
            {
                if (_queue.TryDequeue(out IEnumerable<string>? arguments))
                {
                    //TODO: before execution action

                    await _commandExecutor.ExecuteAsync(arguments, InputOptions, Behavior, cancellationToken);
                    _console.ResetColor();

                    //TODO: after execution action
                }
            }

            return ExitCode.Success;
        }
    }
}
