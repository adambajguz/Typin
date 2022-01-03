namespace Typin.Features
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Binding;
    using Typin.Features.Input;
    using Typin.Models;

    /// <summary>
    /// Binder feature.
    /// </summary>
    public interface IBinderFeature
    {
        /// <summary>
        /// Stores unbounded tokens.
        /// </summary>
        IUnboundedDirectiveCollection UnboundedTokens { get; }

        /// <summary>
        /// A list of bindable models to bind using data from <see cref="UnboundedTokens"/>.
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
        /// Binds <see cref="UnboundedTokens"/> to models in <see cref="Bindable"/>.
        /// This method be called multiple times.
        /// </summary>
        /// <param name="serviceProvider"></param>
        void Bind(IServiceProvider serviceProvider);

        /// <summary>
        /// Validates whether all input, as well as all parameters and required options were bounded.
        /// </summary>
        /// <returns></returns>
        bool Validate();
    }
}
