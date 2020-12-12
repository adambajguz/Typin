namespace Typin.Internal.Input
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Typin.Input;
    using Typin.Internal.Exceptions;
    using Typin.Internal.Extensions;
    using Typin.OptionFallback;
    using Typin.Schemas;

    internal static class CommandBinder
    {
        /// <summary>
        /// Binds parameter inputs in command instance.
        /// </summary>
        public static void BindParameters(this CommandSchema commandSchema, ICommand instance, IReadOnlyList<CommandParameterInput> parameterInputs)
        {
            IReadOnlyList<CommandParameterSchema> parameters = commandSchema.Parameters;

            // All inputs must be bound
            int remainingParameters = parameters.Count;
            int remainingInputs = parameterInputs.Count;

            if (remainingParameters > remainingInputs)
                throw ArgumentBindingExceptions.ParameterNotSet(parameters.TakeLast(remainingParameters - remainingInputs));

            // Scalar parameters
            int i = 0;
            for (; i < parameters.Count && parameters[i].IsScalar; ++i)
            {
                CommandParameterSchema parameter = parameters[i];
                CommandParameterInput scalarInput = parameterInputs[i];

                parameter.BindOn(instance, scalarInput.Value);

                --remainingParameters;
                --remainingInputs;
            }

            // Non-scalar parameter (only one is allowed)
            if (i < parameters.Count && !parameters[i].IsScalar)
            {
                CommandParameterSchema nonScalarParameter = parameters[i];

                string[] nonScalarValues = parameterInputs.TakeLast(remainingInputs)
                                                          .Select(p => p.Value)
                                                          .ToArray();

                // Parameters are required by default and so a non-scalar parameter must be bound to at least one value
                if (!nonScalarValues.Any())
                    throw ArgumentBindingExceptions.ParameterNotSet(new[] { nonScalarParameter });

                nonScalarParameter.BindOn(instance, nonScalarValues);
                --remainingParameters;
                remainingInputs = 0;
            }

            // Ensure all inputs were bound
            if (remainingInputs > 0)
                throw ArgumentBindingExceptions.UnrecognizedParametersProvided(parameterInputs.TakeLast(remainingParameters));
        }

        /// <summary>
        /// Binds option inputs in command instance.
        /// </summary>
        public static void BindOptions(this CommandSchema commandSchema,
                                        ICommand instance,
                                        IReadOnlyList<CommandOptionInput> optionInputs,
                                        IOptionFallbackProvider optionFallbackProvider)
        {
            IReadOnlyList<CommandOptionSchema> options = commandSchema.Options;

            // All inputs must be bound
            HashSet<CommandOptionInput> remainingOptionInputs = optionInputs.ToHashSet();

            // All required options must be set
            HashSet<CommandOptionSchema> unsetRequiredOptions = options.Where(o => o.IsRequired)
                                                                       .ToHashSet();

            // Direct or fallback input
            foreach (CommandOptionSchema option in options)
            {
                IEnumerable<CommandOptionInput> inputs = optionInputs.Where(i => option.MatchesNameOrShortName(i.Alias));

                bool inputsProvided = inputs.Any();

                // Check fallback value
                if (!inputsProvided &&
                    option.FallbackVariableName is string v &&
                    optionFallbackProvider.TryGetValue(v, option.Property!.PropertyType, out string? value))
                {
                    string[] values = option.IsScalar ? new[] { value! } : value!.Split(Path.PathSeparator);

                    option.BindOn(instance, values);
                    unsetRequiredOptions.Remove(option);

                    continue;
                }
                else if (!inputsProvided) // Skip if the inputs weren't provided for this option
                    continue;

                string[] inputValues = inputs.SelectMany(i => i.Values)
                                             .ToArray();

                option.BindOn(instance, inputValues);

                remainingOptionInputs.RemoveRange(inputs);

                // Required option implies that the value has to be set and also be non-empty
                if (inputValues.Any())
                    unsetRequiredOptions.Remove(option);
            }

            // Ensure all inputs were bound
            if (remainingOptionInputs.Any())
                throw ArgumentBindingExceptions.UnrecognizedOptionsProvided(remainingOptionInputs);

            // Ensure all required options were set
            if (unsetRequiredOptions.Any())
                throw ArgumentBindingExceptions.RequiredOptionsNotSet(unsetRequiredOptions);
        }
    }
}
