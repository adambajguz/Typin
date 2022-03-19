namespace Typin.Features
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Binding;

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
        void Add(BindableModel model);

        /// <summary>
        /// Removes a bindable model by identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Whether bindable model with identifier <paramref name="id"/> was removed.</returns>
        bool TryRemove(int id);

        /// <summary>
        /// Removes a bindable model by schema type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Whether bindable model with type <paramref name="type"/> was removed.</returns>
        bool TryRemove(Type type);

        /// <summary>
        /// Get bindable model by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IReadOnlyList<BindableModel> Get(Type type);

        /// <summary>
        /// Get bindable model by identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IReadOnlyList<BindableModel> Get(int id);

        /// <summary>
        /// Binds <see cref="UnboundedTokens"/> to models in <see cref="Bindable"/>.
        /// This method be called multiple times.
        /// </summary>
        /// <param name="serviceProvider"></param>
        void Bind(IServiceProvider serviceProvider);

        /// <summary>
        /// Binds <see cref="UnboundedTokens"/> to model <paramref name="bindableModel"/>.
        /// This method be called multiple times.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="bindableModel"></param>
        void Bind(IServiceProvider serviceProvider, BindableModel bindableModel); //TODO: is this necessary.

        /// <summary>
        /// Validates whether all input, as well as all parameters and required options were bounded.
        /// </summary>
        /// <returns></returns>
        void Validate();
    }
}
