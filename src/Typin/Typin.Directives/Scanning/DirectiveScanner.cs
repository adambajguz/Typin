namespace Typin.Directives.Scanning
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Directives;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="IDirective"/> component scanner.
    /// </summary>
    internal sealed class DirectiveScanner : Scanner<IDirective>, IDirectiveScanner
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="current"></param>
        public DirectiveScanner(IServiceCollection services, IEnumerable<Type>? current) :
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
            return IDirective.IsValidType(type);
        }
    }
}
