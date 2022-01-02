namespace Typin.Features
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Typin.Features.Binding;
    using Typin.Models;

    /// <summary>
    /// Binder feature.
    /// </summary>
    public interface IBinderFeature
    {
        /// <summary>
        /// Stores unbounded input data.
        /// </summary>
        UnboundedInput UnboundedInput { get; }

        /// <summary>
        /// A list of bindable models to bind using data from <see cref="UnboundedInput"/>.
        /// </summary>
        IReadOnlyList<BindableModel> Bindable { get; }

        /// <summary>
        /// Adds <paramref name="model"/> to binder models collection when model of same type wasn't already registered.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Whether <paramref name="model"/> was added.</returns>
        bool TryAdd(BindableModel model);

        /// <summary>
        /// Removes a bindable model.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Whether <paramref name="type"/> was removed.</returns>
        bool TryRemove(Type type);

        /// <summary>
        /// Get bindable model.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        BindableModel? Get(Type type);

        /// <summary>
        /// Get bindable model instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T? Get<T>() where T : class, IModel;

        /// <summary>
        /// Binds <see cref="UnboundedInput"/> to models in <see cref="Bindable"/>.
        /// This method be called multiple times.
        /// </summary>
        void Bind(IConfiguration configuration);

        /// <summary>
        /// Validates whether all input, as well as all parameters and required options were bounded.
        /// </summary>
        /// <returns></returns>
        bool Validate();
    }
}
