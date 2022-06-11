using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Typin.Experimental
{
    // Command Specification

    /// <summary>
    /// This interface represents a type of command where its <see cref="Descriptor"/> describes 
    /// the class of command (with summary, options, arguments, examples, etc.).
    /// 
    /// This command type could represent just a generic description for multiple other sub-commands,
    /// and not be able to run itself. However it could add some description and examples for the entire
    /// collection of sub-commands, and add some common arguments and options accesible for all sub-commands
    /// 
    /// A <see cref="ICommandProvider"/> returns a collection if this type to indicate available 
    /// commands to the Typin runtime.
    /// </summary>
    public interface ICommandType
    {
        /// <summary>
        /// Returns a Command Type description that includes human readable information and
        /// arguments and options specifications
        /// </summary>
        public CommandDescriptor Descriptor { get; }
    }

    /// <summary>
    /// This interface represents a type of command that can perform a specific task. Hence it inherits 
    /// it's description from <see cref="ICommandType"/> and adds through <see cref="CreateRunner"/>
    /// a way to obtain a new instance of a command runner.
    /// </summary>
    public interface IExecutableCommandType : ICommandType
    {
        /// <summary>
        /// Creates and returns a new instance of <see cref="ICommandRunner"/> capable of executing
        /// the task this type represents.
        /// </summary>
        /// <returns></returns>
        ICommandRunner CreateRunner();
    }

    /// <summary>
    /// Describes a <see cref="ICommandType"/> with human readable information and command input specifications
    /// </summary>
    public class CommandDescriptor
    {
        /// <summary>
        /// Default name to invoke the command. It can be a space separated collection of tokens to create
        /// a command hierarchy, like <code>pods</code> and <code>pods list all</code>
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// A short summary able to be printed in a single console line, when the command is presented 
        /// in a list of other sibling commands
        /// </summary>
        public string Summary { get; init; }

        /// <summary>
        /// A description, possibly more extense than the <see cref="Summary"/> decribing what the command does
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// List of argument descriptors specifying the arguments of current command
        /// </summary>
        public IReadOnlyCollection<CommandArgumentDescriptor> Arguments { get; init; }

        /// <summary>
        /// List of option descriptors specifying the options of current command
        /// </summary>
        public IReadOnlyCollection<CommandOptionDescriptor> Options { get; init; }

        /// <summary>
        /// List of examples to show to the user for current command
        /// </summary>
        public IReadOnlyCollection<CommandExampleDescriptor> Examples { get; init; }
    }

    public class CommandArgumentDescriptor
    {
        public string Name { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public int Position { get; init; }
        public bool Multiple { get; init; }
    }

    public enum OptionKind
    {
        Flag,
        Value,
    }

    public class CommandOptionDescriptor
    {
        public string Name { get; init; }
        public IReadOnlyCollection<string> ShortAliases { get; init; }

        /// <summary>
        /// For flags, you can specify a negative name, like no-verbose, no-build, etc., so the user can
        /// explicitly indicate that he wants to give a false value for the flag, disregarding it's default value
        /// </summary>
        public string NegativeName { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public bool Multiple { get; init; }
        public OptionKind OptionKind { get; init; }
        public string DefaultValue { get; init; }
    }

    public class CommandExampleDescriptor
    {
        public string Description { get; init; }
    }

    // Pure Base Runtime

    public interface ICommandRunner
    {
        ValueTask ExecuteAsync(CommandRunnerContext context, CancellationToken cancellationToken);
    }

    public class CommandRunnerContext
    {
        public ICliContext CliContext { get; init; }
        public CommandDescriptor CommandDescriptor { get; init; }
        public string CommandLine { get; init; }
        public IReadOnlyCollection<ParsedArgument> ParsedArguments { get; init; }
        public IReadOnlyCollection<ParsedOption> ParsedOptions { get; init; }
    }

    public class ParsedArgument
    {
        public CommandArgumentDescriptor Descriptor { get; init; }
        public int Position { get; init; }
        public string Value { get; init; }
    }

    public class ParsedOption
    {
        public CommandOptionDescriptor Descriptor { get; init; }
        public string Name { get; init; }
        public string Value { get; init; }
    }

    // Command Providers

    public interface ICommandProvider
    {
        IReadOnlyCollection<ICommandType> Commands { get; }

        event EventHandler<CommandsChangedEventArgs> CommandsChanged;
    }

    public class CommandsChangedEventArgs : EventArgs
    {
        public static readonly CommandsChangedEventArgs Empty = new CommandsChangedEventArgs();
    }

    // Middleware

    public delegate ValueTask CommandMiddlewareFunc(CommandRunnerContext context);

    public interface ICommandMiddleware
    {
        ValueTask HandleAsync(CommandRunnerContext context, CommandMiddlewareFunc next, CancellationToken cancellationToken);
    }
}
