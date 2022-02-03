namespace Typin.Console
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Console collection;
    /// </summary>
    public class ConsoleCollection : ICollection<NamedConsoleDescriptor>
    {
        private readonly ConcurrentDictionary<string, NamedConsoleDescriptor> _consoles = new();

        /// <inheritdoc/>
        public int Count => _consoles.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <summary>
        /// Collection of all console names.
        /// </summary>
        public IEnumerable<string> Names => _consoles.Keys;

        /// <summary>
        /// Gets or sets a given console. Setting a null value removes the console.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested schema, or null if it is not present.</returns>
        public NamedConsoleDescriptor? this[string key]
        {
            get => _consoles.GetValueOrDefault(key);
            set
            {
                if (value is null)
                {
                    _consoles.TryRemove(key, out _);
                }
                else
                {
                    _consoles.AddOrUpdate(value.Name, value, (k, i) => value);
                }
            }
        }

        /// <summary>
        /// Removes all consoles from the collection.
        /// </summary>
        public void Clear()
        {
            _consoles.Clear();
        }

        /// <summary>
        /// Retrieves the requested console from the collection.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested descriptor, or null if it is not present.</returns>
        public NamedConsoleDescriptor? Get(string key)
        {
            return this[key];
        }

        /// <summary>
        /// Sets the given console in the collection.
        /// </summary>
        /// <param name="instance"></param>
        public void Add(NamedConsoleDescriptor instance)
        {
            this[instance.Name] = instance;
        }

        /// <summary>
        /// Sets the given console in the collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lifetime"></param>
        /// <param name="implementationType"></param>
        public void Add(string name, ServiceLifetime lifetime, Type implementationType)
        {
            this[name] = new NamedConsoleDescriptor(name, lifetime, implementationType);
        }

        /// <summary>
        /// Removes a schema in the collection by key.
        /// </summary>
        /// <param name="key">The schema key.</param>
        public bool Remove(string key)
        {
            return _consoles.TryRemove(key, out _);
        }

        /// <inheritdoc/>
        public bool Contains(NamedConsoleDescriptor item)
        {
            return _consoles.Values.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(NamedConsoleDescriptor[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Remove(NamedConsoleDescriptor item)
        {
            return _consoles.TryRemove(new KeyValuePair<string, NamedConsoleDescriptor>(item.Name, item));
        }

        /// <inheritdoc/>
        public IEnumerator<NamedConsoleDescriptor> GetEnumerator()
        {
            return _consoles.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _consoles.Values.GetEnumerator();
        }
    }
}
