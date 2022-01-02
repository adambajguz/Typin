#if !NET6_0_OR_GREATER
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.Extensions.DependencyInjection
{
    using System;

    /// <summary>
    /// Extension methods for getting services from an <see cref="IServiceProvider" />.
    /// </summary>
    public static class AsyncServiceScopeExtensions
    {
        /// <summary>
        /// Creates a new <see cref="AsyncServiceScope"/> that can be used to resolve scoped services.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> to create the scope from.</param>
        /// <returns>An <see cref="AsyncServiceScope"/> that can be used to resolve scoped services.</returns>
        public static AsyncServiceScope CreateAsyncScope(this IServiceProvider provider)
        {
            return new AsyncServiceScope(provider.CreateScope());
        }

        /// <summary>
        /// Creates a new <see cref="AsyncServiceScope"/> that can be used to resolve scoped services.
        /// </summary>
        /// <param name="serviceScopeFactory">The <see cref="IServiceScopeFactory"/> to create the scope from.</param>
        /// <returns>An <see cref="AsyncServiceScope"/> that can be used to resolve scoped services.</returns>
        public static AsyncServiceScope CreateAsyncScope(this IServiceScopeFactory serviceScopeFactory)
        {
            return new AsyncServiceScope(serviceScopeFactory.CreateScope());
        }
    }
}
#endif