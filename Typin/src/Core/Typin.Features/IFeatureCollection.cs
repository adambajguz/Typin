// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//
// https://github.com/dotnet/aspnetcore/blob/main/src/Extensions/Features/src/FeatureCollection.cs
//
namespace Typin.Features
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of CLI functions.
    /// </summary>
    public interface IFeatureCollection : IEnumerable<KeyValuePair<Type, object>>
    {
        /// <summary>
        /// Indicates if the collection can be modified.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Incremented for each modification and can be used to verify cached results.
        /// </summary>
        int Revision { get; }

        /// <summary>
        /// Gets or sets a given feature. Setting a null value removes the feature.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested feature, or null if it is not present.</returns>
        object? this[Type key] { get; set; }

        /// <summary>
        /// Retrieves the requested feature from the collection.
        /// </summary>
        /// <typeparam name="TFeature">The feature key.</typeparam>
        /// <returns>The requested feature, or null if it is not present.</returns>
        TFeature? Get<TFeature>();

        /// <summary>
        /// Sets the given feature in the collection.
        /// </summary>
        /// <typeparam name="TFeature">The feature key.</typeparam>
        /// <param name="instance">The feature value.</param>
        void Set<TFeature>(TFeature? instance);
    }
}
