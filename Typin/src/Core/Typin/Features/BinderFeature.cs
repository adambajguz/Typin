namespace Typin.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Exceptions.ArgumentBinding;
    using Typin.Features.Binder;
    using Typin.Features.Binding;
    using Typin.Features.Input;
    using Typin.Models;
    using Typin.Models.Binding;
    using Typin.Models.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// <see cref="IBinderFeature"/> implementation.
    /// </summary>
    internal sealed class BinderFeature : IBinderFeature
    {
        private readonly Dictionary<Type, BindableModel> _bindableMap = new();
        private readonly List<BindableModel> _bindable = new();

        /// <inheritdoc/>
        public UnboundedInput UnboundedInput { get; }

        /// <inheritdoc/>
        public IReadOnlyList<BindableModel> Bindable => _bindable;

        /// <summary>
        /// Initializes a new instance of <see cref="BinderFeature"/>.
        /// </summary>
        public BinderFeature(UnboundedInput unboundedInput)
        {
            UnboundedInput = unboundedInput;
        }

        /// <inheritdoc/>
        public bool TryAdd(BindableModel model)
        {
            if (_bindableMap.TryAdd(model.Schema.Type, model))
            {
                _bindable.Add(model);

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryRemove(Type type)
        {
            if (_bindableMap.Remove(type, out BindableModel? model))
            {
                _bindable.Remove(model);

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public BindableModel? Get(Type type)
        {
            return _bindableMap.GetValueOrDefault(type);
        }

        /// <inheritdoc/>
        public T? Get<T>()
            where T : class, IModel
        {
            return _bindableMap.GetValueOrDefault(typeof(T))?.Instance as T;
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
            UnboundedInput unboundedInput = UnboundedInput;

            // Ensure all input parameters were bound
            if (unboundedInput.Parameters.Count > 0)
            {
                throw new UnrecognizedParametersException(unboundedInput.Parameters);
            }

            // Ensure all input options were bound
            if (unboundedInput.Options.Count > 0)
            {
                throw new UnrecognizedOptionsException(unboundedInput.Options);
            }

            return true;
        }

        #region Helpers
        /// <summary>
        /// Binds parameter inputs in command instance.
        /// </summary>
        private void BindParameters(IServiceProvider serviceProvider, BindableModel bindableModel)
        {
            IReadOnlyList<ParameterInput> parameterInputs = UnboundedInput.Parameters;
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
            for (; i < parameters.Count && parameters[i].Bindable.IsScalar; ++i)
            {
                IParameterSchema parameter = parameters[i];
                ParameterInput scalarInput = parameterInputs[i];

                parameter.BindOn(serviceProvider, bindableModel.Instance, scalarInput.Value);

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

                nonScalarParameter.BindOn(serviceProvider, bindableModel.Instance, nonScalarValues);
                --remainingParameters;
                remainingInputs = 0;
            }
        }

        /// <summary>
        /// Binds option inputs in command instance.
        /// </summary>
        public void BindOptions(IServiceProvider serviceProvider, BindableModel bindableModel)
        {
            IReadOnlyList<OptionInput> optionInputs = UnboundedInput.Options;
            IReadOnlyList<IOptionSchema> options = bindableModel.Schema.Options;

            // All inputs must be bound
            HashSet<OptionInput> remainingOptionInputs = optionInputs.ToHashSet();

            // All required options must be set
            HashSet<IOptionSchema> unsetRequiredOptions = options.Where(o => o.IsRequired)
                                                                 .ToHashSet();

            // Direct or fallback input
            foreach (OptionSchema option in options)
            {
                IEnumerable<OptionInput> inputs = optionInputs.Where(i => option.MatchesNameOrShortName(i.Alias));

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

                option.BindOn(serviceProvider, bindableModel.Instance, inputValues);

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
                $"{nameof(UnboundedInput)} = {UnboundedInput}";
        }
    }
}
