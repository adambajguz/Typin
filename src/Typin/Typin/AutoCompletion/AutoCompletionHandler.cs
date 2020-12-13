namespace Typin.AutoCompletion
{
    using System.Linq;

    /// <summary>
    /// Default auto completion handler.
    /// /// </summary>
    internal class AutoCompletionHandler : IAutoCompletionHandler
    {
        private readonly IRootSchemaAccessor _rootSchemaAccessor;

        /// <inheritdoc/>
        public char[] Separators { get; set; } = new char[] { ' ' };

        /// <summary>
        /// Initializes an instance of <see cref="AutoCompletionHandler"/>.
        /// </summary>
        public AutoCompletionHandler(IRootSchemaAccessor rootSchemaAccessor)
        {
            _rootSchemaAccessor = rootSchemaAccessor;
        }

        /// <inheritdoc/>
        public string[] GetSuggestions(string text, int index)
        {
            return _rootSchemaAccessor.RootSchema.GetCommandNames().AsParallel()
                                                                   .Where(x => x.StartsWith(text))
                                                                   .OrderBy(x => x)
                                                                   .ToArray();
        }
    }
}
