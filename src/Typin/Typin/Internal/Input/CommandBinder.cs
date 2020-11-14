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
        public static void BindParameters(this CommandSchema commandSchema, ICommand instance, IReadOnlyList<CommandParameterInput> parameterInputs)
        {
            IReadOnlyList<CommandParameterSchema> parameters = commandSchema.Parameters;

            // All inputs must be bound
            List<CommandParameterInput> remainingParameterInputs = parameterInputs.ToList();

            // Scalar parameters
            CommandParameterSchema[] scalarParameters = parameters.OrderBy(p => p.Order)
                                                                                .TakeWhile(p => p.IsScalar)
                                                                                .ToArray();

            for (int i = 0; i < scalarParameters.Length; i++)
            {
                CommandParameterSchema parameter = scalarParameters[i];

                CommandParameterInput scalarInput = i < parameterInputs.Count
                                                        ? parameterInputs[i]
                                                        : throw ArgumentBindingExceptions.ParameterNotSet(parameter);

                parameter.BindOn(instance, scalarInput.Value);
                remainingParameterInputs.Remove(scalarInput);
            }

            // Non-scalar parameter (only one is allowed)
            CommandParameterSchema? nonScalarParameter = parameters.OrderBy(p => p.Order)
                                                                   .FirstOrDefault(p => !p.IsScalar);

            if (nonScalarParameter != null)
            {
                string[] nonScalarValues = parameterInputs.Skip(scalarParameters.Length)
                                                          .Select(p => p.Value)
                                                          .ToArray();

                // Parameters are required by default and so a non-scalar parameter must
                // be bound to at least one value
                if (!nonScalarValues.Any())
                    throw ArgumentBindingExceptions.ParameterNotSet(nonScalarParameter);

                nonScalarParameter.BindOn(instance, nonScalarValues);
                remainingParameterInputs.Clear();
            }

            // Ensure all inputs were bound
            if (remainingParameterInputs.Any())
                throw ArgumentBindingExceptions.UnrecognizedParametersProvided(remainingParameterInputs);
        }

        private static void BindOptions(this CommandSchema commandSchema,
                                        ICommand instance,
                                        IReadOnlyList<CommandOptionInput> optionInputs,
                                        IOptionFallbackProvider optionFallbackProvider)
        {
            IReadOnlyList<CommandOptionSchema> options = commandSchema.Options;

            // All inputs must be bound
            List<CommandOptionInput> remainingOptionInputs = optionInputs.ToList();

            // All required options must be set
            List<CommandOptionSchema> unsetRequiredOptions = options.Where(o => o.IsRequired)
                                                                    .ToList();

            // Direct or fallback input
            foreach (CommandOptionSchema option in options)
            {
                CommandOptionInput[] inputs = optionInputs.Where(i => option.MatchesNameOrShortName(i.Alias))
                                                          .ToArray();

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

        /// <summary>
        /// Binds input in command instance.
        /// </summary>
        public static void Bind(this CommandSchema commandSchema,
                                ICommand instance,
                                CommandInput input,
                                IOptionFallbackProvider optionFallbackProvider)
        {
            BindParameters(commandSchema, instance, input.Parameters);
            BindOptions(commandSchema, instance, input.Options, optionFallbackProvider);
        }
    }
}
