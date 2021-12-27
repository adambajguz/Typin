namespace Typin.Commands.Scanning
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="ICommandHandler"/> component scanner.
    /// </summary>
    internal sealed class CommandHandlerScanner : Scanner<ICommandHandler>, ICommandHandlerScanner
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandHandlerScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="current"></param>
        public CommandHandlerScanner(IServiceCollection services, IEnumerable<Type>? current) :
            base(current)
        {
            _services = services;

            Added += (sender, e) =>
            {
                _services.AddTransient(e.Type);
            };
        }

        /// <inheritdoc/>
        public override bool IsValidComponent(Type type)
        {
            return ICommandHandler.IsValidType(type);
        }
    }
}
