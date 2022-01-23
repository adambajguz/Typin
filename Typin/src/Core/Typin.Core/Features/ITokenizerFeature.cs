namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin.Features.Input;
    using Typin.Features.Tokenizer;

    /// <summary>
    /// Command line input tokenizer feature.
    /// </summary>
    public interface ITokenizerFeature
    {
        /// <summary>
        /// Commadn command line input to tokenize arguments.
        /// </summary>
        IEnumerable<string> Input { get; set; }

        /// <summary>
        /// Command execution options.
        /// </summary>
        InputOptions InputOptions { get; set; }

        /// <summary>
        /// Tokenized CLI input as a set of directives.
        /// </summary>
        IDirectiveCollection? Tokens { get; set; }

        /// <summary>
        /// A collection of token handlers.
        /// For each token in <see cref="Tokens"/> every token handler is executed in order untill it returns <see langword="true"/>.
        /// </summary>
        IList<ITokenHandler> Handlers { get; set; }

        /// <summary>
        /// Tokenizes <see cref="Input"/> using <see cref="InputOptions"/> and sets result to <see cref="Tokens"/>.
        /// </summary>
        /// <returns></returns>
        void Tokenize();

        /// <summary>
        /// Tokenizes provided <paramref name="arguments"/> with <paramref name="options"/> as input options.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IDirectiveCollection Tokenize(IEnumerable<string> arguments, InputOptions options);
    }
}
