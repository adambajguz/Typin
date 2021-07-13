namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting.Components;
    using Typin.Hosting.Components.Internal;

    /// <summary>
    /// CLI builder extensions.
    /// </summary>
    public static class CliBuilderExtensions
    {
        /// <summary>
        /// Adds Typin command line components.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="components"></param>
        /// <returns></returns>
        public static ICliBuilder AddComponents(this ICliBuilder cliBuilder, Action<ICliComponentCollection> components)
        {
            CliComponentCollection cliComponentCollection = new(cliBuilder);
            components(cliComponentCollection);

            ICliComponentProvider provider = cliComponentCollection.Build();
            cliBuilder.Services.AddSingleton(provider);

            return cliBuilder;
        }
    }
}
