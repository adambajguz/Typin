namespace Typin.Plugins.Scopes
{
    using System;

    /// <summary>
    /// Scope manager.
    /// </summary>
    public interface IScopeManager
    {
        /// <summary>
        /// Event invoked when a scope changed.
        /// </summary>
        event EventHandler<ScopeChangedEventArgs>? Changed;

        /// <summary>
        /// Current scope value.
        /// </summary>
        string Current { get; }

        /// <summary>
        /// When application runs in interactive mode, [>] directive followed by command(s) name(s) would scope to the command(s), allowing to omit specified command name(s).
        /// <example>
        /// Commands:
        ///              > [>] cmd1 sub
        ///      cmd1 sub> list
        ///      cmd1 sub> get
        ///              > [>] cmd1
        ///          cmd1> test
        ///          cmd1> -h
        ///
        /// are an equivalent to:
        ///              > cmd1 sub list
        ///              > cmd1 sub get
        ///              > cmd1 test
        ///              > cmd1 -h
        /// </example>
        /// </summary>
        bool Set(string scope);

        /// <summary>
        /// When application runs in interactive mode, [>] directive followed by command(s) name(s) would scope to the command(s), allowing to omit specified command name(s).
        /// <example>
        /// Commands:
        ///              > [>] cmd1 sub
        ///      cmd1 sub> list
        ///      cmd1 sub> get
        ///              > [>] cmd1
        ///          cmd1> test
        ///          cmd1> -h
        ///
        /// are an equivalent to:
        ///              > cmd1 sub list
        ///              > cmd1 sub get
        ///              > cmd1 test
        ///              > cmd1 -h
        /// </example>
        /// </summary>
        bool Set(string[] scope);

        /// <summary>
        /// When application runs in interactive mode, [>] directive followed by command(s) name(s) would scope to the command(s), allowing to omit specified command name(s).
        /// <example>
        /// Commands:
        ///              > [>] cmd1 sub
        ///      cmd1 sub> list
        ///      cmd1 sub> get
        ///              > [>] cmd1
        ///          cmd1> test
        ///          cmd1> -h
        ///
        /// are an equivalent to:
        ///              > cmd1 sub list
        ///              > cmd1 sub get
        ///              > cmd1 test
        ///              > cmd1 -h
        /// </example>
        /// </summary>
        bool Append(string scope);

        /// <summary>
        /// When application runs in interactive mode, [>] directive followed by command(s) name(s) would scope to the command(s), allowing to omit specified command name(s).
        /// <example>
        /// Commands:
        ///              > [>] cmd1 sub
        ///      cmd1 sub> list
        ///      cmd1 sub> get
        ///              > [>] cmd1
        ///          cmd1> test
        ///          cmd1> -h
        ///
        /// are an equivalent to:
        ///              > cmd1 sub list
        ///              > cmd1 sub get
        ///              > cmd1 test
        ///              > cmd1 -h
        /// </example>
        /// </summary>
        bool Append(string[] scope);

        /// <summary>
        /// If application runs in interactive mode, [.] directive can be used to remove one command from the scope.
        /// <example>
        ///             > [>] cmd1 sub
        ///     cmd1 sub> list
        ///     cmd1 sub> [.]
        ///         cmd1>
        /// </example>
        /// </summary>
        bool Up(int by = 1);

        /// <summary>
        /// If application runs in interactive mode, [..] directive can be used to reset current scope to default (global scope).
        /// <example>
        ///             > [>] cmd1 sub
        ///     cmd1 sub> list
        ///     cmd1 sub> [..]
        ///             >
        /// </example>
        /// </summary>
        bool Reset();
    }
}
