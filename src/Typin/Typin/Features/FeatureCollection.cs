﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//
// https://github.com/dotnet/aspnetcore/blob/main/src/Extensions/Features/src/FeatureCollection.cs
//
namespace Typin.Features
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Default implementation for <see cref="IFeatureCollection"/>.
    /// </summary>
    public class FeatureCollection : IFeatureCollection
    {
        private static readonly KeyComparer FeatureKeyComparer = new();
        private readonly IFeatureCollection? _defaults;
        private readonly int _initialCapacity;
        private IDictionary<Type, object>? _features;
        private volatile int _containerRevision;

        /// <inheritdoc />
        public virtual int Revision => _containerRevision + (_defaults?.Revision ?? 0);

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <summary>
        /// Initializes a new instance of <see cref="FeatureCollection"/>.
        /// </summary>
        public FeatureCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="FeatureCollection"/> with the specified initial capacity.
        /// </summary>
        /// <param name="initialCapacity">The initial number of elements that the collection can contain.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="initialCapacity"/> is less than 0</exception>
        public FeatureCollection(int initialCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            }

            _initialCapacity = initialCapacity;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FeatureCollection"/> with the specified defaults.
        /// </summary>
        /// <param name="defaults">The feature defaults.</param>
        public FeatureCollection(IFeatureCollection defaults)
        {
            _defaults = defaults;
        }

        /// <inheritdoc />
        public object? this[Type key]
        {
            get
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                return _features is not null && _features.TryGetValue(key, out var result) ? result : _defaults?[key];
            }
            set
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                if (value == null)
                {
                    if (_features is not null && _features.Remove(key))
                    {
                        ++_containerRevision;
                    }

                    return;
                }

                if (_features == null)
                {
                    _features = new Dictionary<Type, object>(_initialCapacity);
                }
                _features[key] = value;
                ++_containerRevision;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            if (_features is not null)
            {
                foreach (var pair in _features)
                {
                    yield return pair;
                }
            }

            if (_defaults is not null)
            {
                // Don't return features masked by the wrapper.
                foreach (var pair in _features == null ? _defaults : _defaults.Except(_features, FeatureKeyComparer))
                {
                    yield return pair;
                }
            }
        }

        /// <inheritdoc />
        public TFeature? Get<TFeature>()
        {
            return (TFeature?)this[typeof(TFeature)];
        }

        /// <inheritdoc />
        public void Set<TFeature>(TFeature? instance)
        {
            this[typeof(TFeature)] = instance;
        }

        private class KeyComparer : IEqualityComparer<KeyValuePair<Type, object>>
        {
            public bool Equals(KeyValuePair<Type, object> x, KeyValuePair<Type, object> y)
            {
                return x.Key.Equals(y.Key);
            }

            public int GetHashCode(KeyValuePair<Type, object> obj)
            {
                return obj.Key.GetHashCode();
            }
        }
    }
}