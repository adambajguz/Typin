namespace Typin.AutoCompletion
{
    using System.Linq;

    /// <summary>
    /// Default auto completion handler.
    /// /// </summary>
    internal class AutoCompletionHandler : IAutoCompletionHandler
    {
        private readonly ICliContext _cliContext;

        /// <inheritdoc/>
        public char[] Separators { get; set; } = new char[] { ' ' };

        /// <summary>
        /// Initializes an instance of <see cref="AutoCompletionHandler"/>.
        /// </summary>
        public AutoCompletionHandler(ICliContext cliContext)
        {
            _cliContext = cliContext;
        }

        /// <inheritdoc/>
        public string[] GetSuggestions(string text, int index)
        {
            string[] vs = _cliContext.RootSchema.GetCommandNames().AsParallel()
                                                                  .Where(x => x.StartsWith(text))
                                                                  .OrderBy(x => x)
                                                                  //.Select(x => x.Substring(text.Length))
                                                                  .ToArray();
            return vs;
        }
    }
}
