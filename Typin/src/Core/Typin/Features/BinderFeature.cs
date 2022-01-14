namespace Typin.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Exceptions.ArgumentBinding;
    using Typin.Features.Binder;
    using Typin.Features.Binding;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;
    using Typin.Models;
    using Typin.Models.Binding;
    using Typin.Models.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// <see cref="IBinderFeature"/> implementation.
    /// </summary>
    internal sealed class BinderFeature : IBinderFeature
    {
        private readonly Dictionary<Type, BindableModel> _bindableMapByType = new();
        private readonly Dictionary<int, BindableModel> _bindableMapById = new();
        private readonly List<BindableModel> _bindable = new();

        /// <inheritdoc/>
        public IUnboundedDirectiveCollection UnboundedTokens { get; }

        /// <inheritdoc/>
        public IReadOnlyList<BindableModel> Bindable => _bindable;

        /// <summary>
        /// Initializes a new instance of <see cref="BinderFeature"/>.
        /// </summary>
        public BinderFeature(IDirectiveCollection tokensToBind)
        {
            UnboundedTokens = new UnboundedDirectiveCollection(tokensToBind);
        }

        /// <inheritdoc/>
        public bool TryAdd(BindableModel model)
        {
            if (_bindableMapByType.TryAdd(model.Schema.Type, model))
            {
                _bindable.Add(model);
                _bindableMapById.Add(model.Id, model);

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryRemove(int id)
        {
            if (_bindableMapById.Remove(id, out BindableModel? model))
            {
                _bindable.Remove(model);
                _bindableMapByType.Remove(model.Schema.Type);

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryRemove(Type type)
        {
            if (_bindableMapByType.Remove(type, out BindableModel? model))
            {
                _bindable.Remove(model);
                _bindableMapById.Remove(model.Id);

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public BindableModel? Get(Type type)
        {
            return _bindableMapByType.GetValueOrDefault(type);
        }

        /// <inheritdoc/>
        public BindableModel? Get(int id)
        {
            return _bindableMapById.GetValueOrDefault(id);
        }

        /// <inheritdoc/>
        public T? Get<T>()
            where T : class, IModel
        {
            return _bindableMapByType.GetValueOrDefault(typeof(T))?.Instance as T;
        }

        /// <inheritdoc/>
        public void Bind(IServiceProvider serviceProvider)
        {
            foreach (BindableModel bindableModel in Bindable)
            {
                BindParameters(serviceProvider, bindableModel);
                BindOptions(serviceProvider, bindableModel);
            }
        }

        /// <inheritdoc/>
        public bool Validate()
        {
            // Ensure all tokens were bounded
            if (UnboundedTokens.Count > 0)
            {
                throw new UnboundedTokensException();
            }

            return true;
        }

        #region Helpers
        /// <summary>
        /// Binds parameter inputs in command instance.
        /// </summary>
        private void BindParameters(IServiceProvider serviceProvider, BindableModel bindableModel)
        {
            TokenGroup<ValueToken>? parameterGroup = UnboundedTokens[0].Children?.Get<ValueToken>();

            if (parameterGroup is null)
            {
                return;
            }

            IList<ValueToken>? parameterInputs = parameterGroup.Tokens;
            IReadOnlyList<IParameterSchema> parameters = bindableModel.Schema.Parameters;

            // All inputs must be bound
            int remainingParameters = parameters.Count;
            int remainingInputs = parameterInputs.Count;

            if (remainingParameters > remainingInputs)
            {
                throw new MissingParametersException(parameters.TakeLast(remainingParameters - remainingInputs));
            }

            // Scalar parameters
            int i = 0;
            for (; i < parameters.Count && parameters[i].Bindable.IsScalar; i++)
            {
                IParameterSchema parameter = parameters[i];
                ValueToken scalarInput = parameterInputs[i];

                parameter.BindOn(serviceProvider, bindableModel, scalarInput.Value);

                --remainingParameters;
                --remainingInputs;
            }

            // Non-scalar parameter (only one is allowed)
            if (i < parameters.Count && !parameters[i].Bindable.IsScalar)
            {
                IParameterSchema nonScalarParameter = parameters[i];

                string[] nonScalarValues = parameterInputs.TakeLast(remainingInputs)
                                                          .Select(p => p.Value)
                                                          .ToArray();

                // Parameters are required by default and so a non-scalar parameter must be bound to at least one value
                if (!nonScalarValues.Any())
                {
                    throw new MissingParametersException(nonScalarParameter);
                }

                nonScalarParameter.BindOn(serviceProvider, bindableModel, nonScalarValues);
                --remainingParameters;
                remainingInputs = 0;
            }
        }

        /// <summary>
        /// Binds option inputs in command instance.
        /// </summary>
        public void BindOptions(IServiceProvider serviceProvider, BindableModel bindableModel)
        {
            TokenGroup<NamedToken>? optionGroup = UnboundedTokens[0].Children?.Get<NamedToken>();

            if (optionGroup is null)
            {
                return;
            }

            IList<NamedToken>? optionInputs = optionGroup.Tokens;
            IReadOnlyList<IOptionSchema> options = bindableModel.Schema.Options;

            // All inputs must be bound
            HashSet<NamedToken> remainingOptionInputs = optionInputs.ToHashSet();

            // All required options must be set
            HashSet<IOptionSchema> unsetRequiredOptions = options.Where(o => o.IsRequired)
                                                                 .ToHashSet();

            // Direct or fallback input
            foreach (OptionSchema option in options)
            {
                IEnumerable<NamedToken> inputs = optionInputs.Where(i => option.MatchesNameOrShortName(i.Alias));

                bool inputsProvided = inputs.Any();

                if (!inputsProvided) // Skip if the inputs weren't provided for this option
                {
                    if (option.Bindable.Kind == BindableArgumentKind.Dynamic)
                    {
                        option.Bindable.SetValue(bindableModel.Instance, null);
                    }

                    continue;
                }

                string[] inputValues = inputs.SelectMany(i => i.Values)
                                             .ToArray();

                option.BindOn(serviceProvider, bindableModel, inputValues);

                remainingOptionInputs.RemoveRange(inputs);

                // Required option implies that the value has to be set and also be non-empty
                if (inputValues.Any())
                {
                    unsetRequiredOptions.Remove(option);
                }
            }

            // Ensure all required options were set
            if (unsetRequiredOptions.Any())
            {
                throw new RequiredOptionsMissingException(unsetRequiredOptions);
            }
        }
        #endregion

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(UnboundedTokens)} = {UnboundedTokens}";
        }
    }
}
