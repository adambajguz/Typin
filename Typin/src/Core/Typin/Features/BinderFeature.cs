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
    using Typin.Models.Binding;
    using Typin.Models.Schemas;

    /// <summary>
    /// <see cref="IBinderFeature"/> implementation.
    /// </summary>
    public sealed class BinderFeature : IBinderFeature
    {
        private readonly Dictionary<Type, List<BindableModel>> _bindableMapByType = new();
        private readonly Dictionary<int, List<BindableModel>> _bindableMapById = new();
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
        public void Add(BindableModel model)
        {
            if (_bindableMapById.TryGetValue(model.DirectiveId, out List<BindableModel>? byId))
            {
                byId.Add(model);
            }
            else
            {
                _bindableMapById.Add(model.DirectiveId, new List<BindableModel> { model });
            }

            Type schemaType = model.Schema.Type;
            if (_bindableMapByType.TryGetValue(schemaType, out List<BindableModel>? byType))
            {
                byType.Add(model);
            }
            else
            {
                _bindableMapByType.Add(schemaType, new List<BindableModel> { model });
            }

            _bindable.Add(model);
        }

        /// <inheritdoc/>
        public bool TryRemove(int id)
        {
            if (_bindableMapById.Remove(id, out List<BindableModel>? models))
            {
                foreach (BindableModel model in models)
                {
                    _bindable.Remove(model);
                    _bindableMapByType.Remove(model.Schema.Type);
                }

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryRemove(Type type)
        {
            if (_bindableMapByType.Remove(type, out List<BindableModel>? models))
            {
                foreach (BindableModel model in models)
                {
                    _bindable.Remove(model);
                    _bindableMapById.Remove(model.DirectiveId);
                }

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public IReadOnlyList<BindableModel> Get(Type type)
        {
            return _bindableMapByType.GetValueOrDefault(type) ?? new List<BindableModel>();
        }

        /// <inheritdoc/>
        public IReadOnlyList<BindableModel> Get(int id)
        {
            return _bindableMapById.GetValueOrDefault(id) ?? new List<BindableModel>();
        }

        /// <inheritdoc/>
        public void Bind(IServiceProvider serviceProvider)
        {
            if (UnboundedTokens.IsBounded)
            {
                return;
            }

            foreach (BindableModel bindableModel in Bindable)
            {
                Bind(serviceProvider, bindableModel);
            }
        }

        /// <inheritdoc/>
        public void Bind(IServiceProvider serviceProvider, BindableModel bindableModel)
        {
            if (UnboundedTokens.IsBounded)
            {
                return;
            }

            IUnboundedDirectiveToken? unboudnedDirectiveTokens = UnboundedTokens[bindableModel.DirectiveId];

            if (unboudnedDirectiveTokens is { HasUnbounded: true })
            {
                BindParameters(serviceProvider, bindableModel, unboudnedDirectiveTokens);
                BindOptions(serviceProvider, bindableModel, unboudnedDirectiveTokens);
            }
        }

        /// <inheritdoc/>
        public void Validate()
        {
            // Ensure all tokens were bounded
            if (UnboundedTokens.HasUnbounded)
            {
                throw new UnboundedTokensException(UnboundedTokens);
            }
        }

        #region Helpers
        /// <summary>
        /// Binds parameter inputs in command instance.
        /// </summary>
        private static void BindParameters(IServiceProvider serviceProvider, BindableModel bindableModel, IUnboundedDirectiveToken unboundedDirectiveTokens)
        {
            TokenGroup<ValueToken>? parameterGroup = unboundedDirectiveTokens.Children.Get<ValueToken>();

            if (parameterGroup is null)
            {
                return;
            }

            IList<ValueToken>? parameterInputs = parameterGroup.Tokens; //TODO: remove tokens after binding
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
        public static void BindOptions(IServiceProvider serviceProvider, BindableModel bindableModel, IUnboundedDirectiveToken unboundedDirectiveTokens)
        {
            TokenGroup<NamedToken>? optionGroup = unboundedDirectiveTokens.Children.Get<NamedToken>();

            if (optionGroup is null)
            {
                return;
            }

            IList<NamedToken> optionInputs = optionGroup.Tokens;
            IReadOnlyList<IOptionSchema> requiredOptions = bindableModel.Schema.RequiredOptions;
            IReadOnlyList<IOptionSchema> options = bindableModel.Schema.Options;

            var unsetRequiredOptions = requiredOptions.ToHashSet();

            // Direct or fallback input
            foreach (OptionSchema option in requiredOptions.Concat(options))
            {
                IEnumerable<NamedToken> inputs = optionInputs.Where(i => option.MatchesNameOrShortName(i.Alias)); //TODO: remove tokens after binding

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

                unsetRequiredOptions.Remove(option);
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
                $"{nameof(UnboundedTokens)} = {{{UnboundedTokens}}}";
        }
    }
}
