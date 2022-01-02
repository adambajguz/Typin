namespace Typin.Plugins.Scopes
{
    using System;
    using Typin.Commands.Collections;

    /// <summary>
    /// Default implementation of <see cref="IScopeManager"/>.
    /// </summary>
    public class ScopesManager : IScopeManager
    {
        private readonly ICommandSchemaCollection _commandSchemas;

        /// <inheritdoc/>
        public string Current { get; private set; } = string.Empty;

        /// <summary>
        /// Initializes an instance of <see cref="ScopesManager"/>.
        /// </summary>
        public ScopesManager(ICommandSchemaCollection commandSchemas)
        {
            _commandSchemas = commandSchemas;
        }

        /// <inheritdoc/>
        public bool Set(string scope)
        {
            scope = scope.Trim();

            if (!_commandSchemas.IsCommandOrSubcommandPart(scope))
            {
                return false;
            }

            Current = scope;

            return true;
        }

        /// <inheritdoc/>
        public bool Set(string[] scope)
        {
            return Set(string.Join(' ', scope));
        }

        /// <inheritdoc/>
        public bool Append(string scope)
        {
            return Set(string.Concat(Current, " ", scope));
        }

        /// <inheritdoc/>
        public bool Append(string[] scope)
        {
            return Append(string.Join(' ', scope));
        }

        /// <inheritdoc/>
        public bool Up(int by = 1)
        {
            if (by < 0)
            {
                by = 0;
            }

            if (by == 0)
            {
                return false;
            }

            string[] splittedScope = Current.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (by >= splittedScope.Length)
            {
                return Reset();
            }

            if (splittedScope.Length > 1)
            {
                Current = string.Join(" ", splittedScope, 0, splittedScope.Length - by);

                return true;
            }

            return Reset();
        }

        /// <inheritdoc/>
        public bool Reset()
        {
            if (!string.IsNullOrEmpty(Current))
            {
                Current = string.Empty;

                return true;
            }

            return false;
        }
    }
}
