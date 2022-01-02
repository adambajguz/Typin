namespace Typin.Models.Scanning
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting.Scanning;
    using Typin.Models;

    /// <summary>
    /// <see cref="IModel"/> component scanner.
    /// </summary>
    internal sealed class ModelScanner : Scanner<IModel>, IModelScanner
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of <see cref="ModelScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="current"></param>
        public ModelScanner(IServiceCollection services, IEnumerable<Type>? current) :
            base(current)
        {
            _services = services;

            Added += (sender, e) =>
            {
                _services.AddTransient(typeof(IModel), e.Type);
                _services.AddTransient(e.Type);
            };
        }

        /// <inheritdoc/>
        public override bool IsValid(Type type)
        {
            return IModel.IsValidType(type);
        }
    }
}
