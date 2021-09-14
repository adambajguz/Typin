namespace Typin.Internal.Input
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Typin.Exceptions.ArgumentBinding;
    using Typin.Input;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    internal static class CommandBinder
    {
        /// <summary>
        /// Binds parameter inputs in command instance.
        /// </summary>
        public static void BindParameters(this CommandSchema commandSchema, ICommand instance, CommandInput input)
        {
            IReadOnlyList<CommandParameterInput> parameterInputs = input.Parameters;
            IReadOnlyList<ParameterSchema> parameters = commandSchema.Parameters;

            // All inputs must be bound
            int remainingParameters = parameters.Count;
            int remainingInputs = parameterInputs.Count;

            if (remainingParameters > remainingInputs)
            {
                throw new MissingParametersException(input, parameters.TakeLast(remainingParameters - remainingInputs));
            }

            // Scalar parameters
            int i = 0;
            for (; i < parameters.Count && parameters[i].Bindable.IsScalar; ++i)
            {
                ParameterSchema parameter = parameters[i];
                CommandParameterInput scalarInput = parameterInputs[i];

                parameter.BindOn(instance, input, scalarInput.Value);

                --remainingParameters;
                --remainingInputs;
            }

            // Non-scalar parameter (only one is allowed)
            if (i < parameters.Count && !parameters[i].Bindable.IsScalar)
            {
                ParameterSchema nonScalarParameter = parameters[i];

                string[] nonScalarValues = parameterInputs.TakeLast(remainingInputs)
                                                          .Select(p => p.Value)
                                                          .ToArray();

                // Parameters are required by default and so a non-scalar parameter must be bound to at least one value
                if (!nonScalarValues.Any())
                {
                    throw new MissingParametersException(input, nonScalarParameter);
                }

                nonScalarParameter.BindOn(instance, input, nonScalarValues);
                --remainingParameters;
                remainingInputs = 0;
            }

            // Ensure all inputs were bound
            if (remainingInputs > 0)
            {
                throw new UnrecognizedParametersException(input, parameterInputs.TakeLast(remainingInputs));
            }
        }

        /// <summary>
        /// Binds option inputs in command instance.
        /// </summary>
        public static void BindOptions(this CommandSchema commandSchema,
                                       ICommand instance,
                                       CommandInput input,
                                       IConfiguration configuration)
        {
            IReadOnlyList<CommandOptionInput> optionInputs = input.Options;
            IReadOnlyList<OptionSchema> options = commandSchema.Options;

            // All inputs must be bound
            HashSet<CommandOptionInput> remainingOptionInputs = optionInputs.ToHashSet();

            // All required options must be set
            HashSet<OptionSchema> unsetRequiredOptions = options.Where(o => o.IsRequired)
                                                                .ToHashSet();

            // Direct or fallback input
            foreach (OptionSchema option in options)
            {
                IEnumerable<CommandOptionInput> inputs = optionInputs.Where(i => option.MatchesNameOrShortName(i.Alias));

                bool inputsProvided = inputs.Any();

                // Check fallback value
                if (!inputsProvided &&
                    option.FallbackVariableName is string v &&
                    configuration[v] is string value)
                {
                    string[] values = option.Bindable.IsScalar ? new[] { value! } : value!.Split(Path.PathSeparator);

                    option.BindOn(instance, input, values);
                    unsetRequiredOptions.Remove(option);

                    continue;
                }
                else if (!inputsProvided) // Skip if the inputs weren't provided for this option
                {
                    if (option.Bindable.Kind == BindableArgumentKind.Dynamic)
                    {
                        option.Bindable.SetValue(instance, null);
                    }

                    continue;
                }

                string[] inputValues = inputs.SelectMany(i => i.Values)
                                             .ToArray();

                option.BindOn(instance, input, inputValues);

                remainingOptionInputs.RemoveRange(inputs);

                // Required option implies that the value has to be set and also be non-empty
                if (inputValues.Any())
                {
                    unsetRequiredOptions.Remove(option);
                }
            }

            // Ensure all inputs were bound
            if (remainingOptionInputs.Any())
            {
                throw new UnrecognizedOptionsException(input, remainingOptionInputs);
            }

            // Ensure all required options were set
            if (unsetRequiredOptions.Any())
            {
                throw new RequiredOptionsMissingException(input, unsetRequiredOptions);
            }
        }
    }
}
