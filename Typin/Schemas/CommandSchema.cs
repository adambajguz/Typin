namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Typin.Attributes;
    using Typin.Input;
    using Typin.Internal.Exceptions;
    using Typin.Internal.Extensions;
    using Typin.OptionFallback;

    /// <summary>
    /// Stores command schema.
    /// </summary>
    public class CommandSchema
    {
        /// <summary>
        /// Command type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Command name.
        /// If the name is not set, the command is treated as a default command, i.e. the one that gets executed when the user
        /// does not specify a command name in the arguments.
        /// All commands in an application must have different names. Likewise, only one command without a name is allowed.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Whether command is a default command.
        /// </summary>
        public bool IsDefault => string.IsNullOrWhiteSpace(Name);

        /// <summary>
        /// Command description, which is used in help text.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Command manual text, which is used in help text.
        /// </summary>
        public string? Manual { get; }

        /// <summary>
        /// Whether command can run only in interactive mode.
        /// </summary>
        public bool InteractiveModeOnly { get; }

        /// <summary>
        /// List of parameters.
        /// </summary>
        public IReadOnlyList<CommandParameterSchema> Parameters { get; }

        /// <summary>
        /// List of options.
        /// </summary>
        public IReadOnlyList<CommandOptionSchema> Options { get; }

        /// <summary>
        /// Whether help option is available for this command.
        /// </summary>
        public bool IsHelpOptionAvailable => Options.Contains(CommandOptionSchema.HelpOption);

        /// <summary>
        /// Whether version option is available for this command.
        /// </summary>
        public bool IsVersionOptionAvailable => Options.Contains(CommandOptionSchema.VersionOption);

        #region ctor
        /// <summary>
        /// Initializes an instance of <see cref="CommandSchema"/>.
        /// </summary>
        private CommandSchema(Type type,
                              string? name,
                              string? description,
                              string? manual,
                              bool interactiveModeOnly,
                              IReadOnlyList<CommandParameterSchema> parameters,
                              IReadOnlyList<CommandOptionSchema> options)
        {
            Type = type;
            Name = name;
            Description = description;
            Manual = manual;
            InteractiveModeOnly = interactiveModeOnly;
            Parameters = parameters;
            Options = options;
        }

        /// <summary>
        /// Resolves <see cref="CommandSchema"/>.
        /// </summary>
        internal static CommandSchema? TryResolve(Type type)
        {
            if (!IsCommandType(type))
                return null;

            CommandAttribute attribute = type.GetCustomAttribute<CommandAttribute>()!;

            string? name = attribute.Name;

            CommandOptionSchema[] builtInOptions = string.IsNullOrWhiteSpace(name)
                ? new[] { CommandOptionSchema.HelpOption, CommandOptionSchema.VersionOption }
                : new[] { CommandOptionSchema.HelpOption };

            CommandParameterSchema?[] parameters = type.GetProperties()
                                                       .Select(CommandParameterSchema.TryResolve)
                                                       .Where(p => p != null)
                                                       .ToArray();

            CommandOptionSchema?[] options = type.GetProperties()
                                                 .Select(CommandOptionSchema.TryResolve)
                                                 .Where(o => o != null)
                                                 .Concat(builtInOptions)
                                                 .ToArray();

            CommandSchema command = new CommandSchema(
                type,
                name,
                attribute.Description,
                attribute.Manual,
                attribute.InteractiveModeOnly,
                parameters!,
                options!
            );

            ValidateParameters(command);
            ValidateOptions(command);

            return command;
        }
        #endregion

        /// <summary>
        /// Enumerates through parameters and options.
        /// </summary>
        public IEnumerable<ArgumentSchema> GetArguments()
        {
            foreach (CommandParameterSchema parameter in Parameters)
                yield return parameter;

            foreach (CommandOptionSchema option in Options)
                yield return option;
        }

        /// <summary>
        /// Returns dictionary of arguments and its values.
        /// </summary>
        public IReadOnlyDictionary<ArgumentSchema, object?> GetArgumentValues(ICommand instance)
        {
            var result = new Dictionary<ArgumentSchema, object?>();

            foreach (ArgumentSchema argument in GetArguments())
            {
                // Skip built-in arguments
                if (argument.Property is null)
                    continue;

                object? value = argument.Property.GetValue(instance);
                result[argument] = value;
            }

            return result;
        }

        #region Bind
        private void BindParameters(ICommand instance, IReadOnlyList<CommandParameterInput> parameterInputs)
        {
            // All inputs must be bound
            List<CommandParameterInput> remainingParameterInputs = parameterInputs.ToList();

            // Scalar parameters
            CommandParameterSchema[] scalarParameters = Parameters.OrderBy(p => p.Order)
                                                                  .TakeWhile(p => p.IsScalar)
                                                                  .ToArray();

            for (int i = 0; i < scalarParameters.Length; i++)
            {
                CommandParameterSchema parameter = scalarParameters[i];

                CommandParameterInput scalarInput = i < parameterInputs.Count
                                                        ? parameterInputs[i]
                                                        : throw EndUserTypinExceptions.ParameterNotSet(parameter);

                parameter.BindOn(instance, scalarInput.Value);
                remainingParameterInputs.Remove(scalarInput);
            }

            // Non-scalar parameter (only one is allowed)
            CommandParameterSchema nonScalarParameter = Parameters.OrderBy(p => p.Order)
                                                                  .FirstOrDefault(p => !p.IsScalar);

            if (nonScalarParameter != null)
            {
                string[] nonScalarValues = parameterInputs.Skip(scalarParameters.Length)
                                                          .Select(p => p.Value)
                                                          .ToArray();

                // Parameters are required by default and so a non-scalar parameter must
                // be bound to at least one value
                if (!nonScalarValues.Any())
                    throw EndUserTypinExceptions.ParameterNotSet(nonScalarParameter);

                nonScalarParameter.BindOn(instance, nonScalarValues);
                remainingParameterInputs.Clear();
            }

            // Ensure all inputs were bound
            if (remainingParameterInputs.Any())
                throw EndUserTypinExceptions.UnrecognizedParametersProvided(remainingParameterInputs);
        }

        private void BindOptions(ICommand instance,
                                 IReadOnlyList<CommandOptionInput> optionInputs,
                                 IOptionFallbackProvider optionFallbackProvider)
        {
            // All inputs must be bound
            List<CommandOptionInput> remainingOptionInputs = optionInputs.ToList();

            // All required options must be set
            List<CommandOptionSchema> unsetRequiredOptions = Options.Where(o => o.IsRequired)
                                                                    .ToList();

            // Direct or fallback input
            foreach (CommandOptionSchema option in Options)
            {
                CommandOptionInput[] inputs = optionInputs.Where(i => option.MatchesNameOrShortName(i.Alias))
                                                          .ToArray();

                bool inputsProvided = inputs.Any();

                // Check fallback value
                if (!inputsProvided &&
                    option.FallbackVariableName is string v &&
                    optionFallbackProvider.TryGetValue(v, option.Property!.PropertyType, out string value))
                {
                    string[] values = option.IsScalar ? new[] { value } : value.Split(Path.PathSeparator);

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
                throw EndUserTypinExceptions.UnrecognizedOptionsProvided(remainingOptionInputs);

            // Ensure all required options were set
            if (unsetRequiredOptions.Any())
                throw EndUserTypinExceptions.RequiredOptionsNotSet(unsetRequiredOptions);
        }

        internal void Bind(ICommand instance,
                           CommandInput input,
                           IOptionFallbackProvider optionFallbackProvider)
        {
            BindParameters(instance, input.Parameters);
            BindOptions(instance, input.Options, optionFallbackProvider);
        }
        #endregion

        internal string GetInternalDisplayString()
        {
            var buffer = new StringBuilder();

            // Type
            buffer.Append(Type.FullName);

            // Name
            buffer.Append(' ')
                  .Append('(')
                  .Append(IsDefault ? "<default command>" : $"'{Name}'")
                  .Append(')');

            return buffer.ToString();
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return GetInternalDisplayString();
        }

        #region Helpers
        private static void ValidateParameters(CommandSchema command)
        {
            IGrouping<int, CommandParameterSchema>? duplicateOrderGroup = command.Parameters
                                                                                 .GroupBy(a => a.Order)
                                                                                 .FirstOrDefault(g => g.Count() > 1);

            if (duplicateOrderGroup != null)
            {
                throw InternalTypinExceptions.ParametersWithSameOrder(
                    command,
                    duplicateOrderGroup.Key,
                    duplicateOrderGroup.ToArray()
                );
            }

            IGrouping<string, CommandParameterSchema>? duplicateNameGroup = command.Parameters
                                                                                   .Where(a => !string.IsNullOrWhiteSpace(a.Name))
                                                                                   .GroupBy(a => a.Name!, StringComparer.OrdinalIgnoreCase)
                                                                                   .FirstOrDefault(g => g.Count() > 1);

            if (duplicateNameGroup != null)
            {
                throw InternalTypinExceptions.ParametersWithSameName(
                    command,
                    duplicateNameGroup.Key,
                    duplicateNameGroup.ToArray()
                );
            }

            CommandParameterSchema[]? nonScalarParameters = command.Parameters
                                                                   .Where(p => !p.IsScalar)
                                                                   .ToArray();

            if (nonScalarParameters.Length > 1)
            {
                throw InternalTypinExceptions.TooManyNonScalarParameters(
                    command,
                    nonScalarParameters
                );
            }

            CommandParameterSchema? nonLastNonScalarParameter = command.Parameters
                                                                       .OrderByDescending(a => a.Order)
                                                                       .Skip(1)
                                                                       .LastOrDefault(p => !p.IsScalar);

            if (nonLastNonScalarParameter != null)
            {
                throw InternalTypinExceptions.NonLastNonScalarParameter(
                    command,
                    nonLastNonScalarParameter
                );
            }
        }

        private static void ValidateOptions(CommandSchema command)
        {
            IEnumerable<CommandOptionSchema> noNameGroup = command.Options
                .Where(o => o.ShortName == null && string.IsNullOrWhiteSpace(o.Name));

            if (noNameGroup.Any())
            {
                throw InternalTypinExceptions.OptionsWithNoName(
                    command,
                    noNameGroup.ToArray()
                );
            }

            CommandOptionSchema[] invalidLengthNameGroup = command.Options
                .Where(o => !string.IsNullOrWhiteSpace(o.Name))
                .Where(o => o.Name!.Length <= 1)
                .ToArray();

            if (invalidLengthNameGroup.Any())
            {
                throw InternalTypinExceptions.OptionsWithInvalidLengthName(
                    command,
                    invalidLengthNameGroup
                );
            }

            IGrouping<string, CommandOptionSchema>? duplicateNameGroup = command.Options
                .Where(o => !string.IsNullOrWhiteSpace(o.Name))
                .GroupBy(o => o.Name!, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateNameGroup != null)
            {
                throw InternalTypinExceptions.OptionsWithSameName(
                    command,
                    duplicateNameGroup.Key,
                    duplicateNameGroup.ToArray()
                );
            }

            IGrouping<char, CommandOptionSchema>? duplicateShortNameGroup = command.Options
                .Where(o => o.ShortName != null)
                .GroupBy(o => o.ShortName!.Value)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateShortNameGroup != null)
            {
                throw InternalTypinExceptions.OptionsWithSameShortName(
                    command,
                    duplicateShortNameGroup.Key,
                    duplicateShortNameGroup.ToArray()
                );
            }

            IGrouping<string, CommandOptionSchema>? duplicateEnvironmentVariableNameGroup = command.Options
                .Where(o => !string.IsNullOrWhiteSpace(o.FallbackVariableName))
                .GroupBy(o => o.FallbackVariableName!, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateEnvironmentVariableNameGroup != null)
            {
                throw InternalTypinExceptions.OptionsWithSameEnvironmentVariableName(
                    command,
                    duplicateEnvironmentVariableNameGroup.Key,
                    duplicateEnvironmentVariableNameGroup.ToArray()
                );
            }
        }

        internal static bool IsCommandType(Type type)
        {
            return type.Implements(typeof(ICommand)) &&
                   type.IsDefined(typeof(CommandAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }
        #endregion
    }
}