namespace Typin.Commands.Scanning
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Commands;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="ICommand"/> component scanner.
    /// </summary>
    internal sealed class CommandScanner : Scanner<ICommand>, ICommandScanner
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="current"></param>
        public CommandScanner(IServiceCollection services, IEnumerable<Type>? current) :
            base(current)
        {
            _services = services;

            Added += (sender, e) =>
            {
                _services.AddTransient(e.Type);
            };
        }

        /// <inheritdoc/>
        public override bool IsValid(Type type)
        {
            return ICommand.IsValidType(type);
        }
    }
}
