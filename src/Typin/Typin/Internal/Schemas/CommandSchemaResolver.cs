namespace Typin.Internal.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Internal.Exceptions;
    using Typin.Schemas;

    /// <summary>
    /// Resolves an instance of <see cref="CommandSchema"/>.
    /// </summary>
    internal static class CommandSchemaResolver
    {
        /// <summary>
        /// Resolves <see cref="CommandSchema"/>.
        /// </summary>
        public static CommandSchema Resolve(Type type, IReadOnlyList<Type>? modeTypes)
        {
            if (!KnownTypesHelpers.IsCommandType(type))
            {
                throw CommandResolverExceptions.InvalidCommandType(type);
            }

            CommandAttribute attribute = type.GetCustomAttribute<CommandAttribute>()!;

            if (modeTypes is not null)
            {
                if (attribute.SupportedModes is not null && attribute.SupportedModes.Except(modeTypes).Any())
                {
                    throw CommandResolverExceptions.InvalidSupportedModesInCommand(type, attribute);
                }

                if (attribute.ExcludedModes is not null && attribute.ExcludedModes.Except(modeTypes).Any())
                {
                    throw CommandResolverExceptions.InvalidExcludedModesInCommand(type, attribute);
                }
            }

            string? name = attribute.Name;

            OptionSchema[] builtInOptions = string.IsNullOrWhiteSpace(name)
                ? new[] { OptionSchema.HelpOption, OptionSchema.VersionOption }
                : new[] { OptionSchema.HelpOption };

            BaseCommandSchema baseSchema = ResolveBase(type, builtInOptions);

            CommandSchema command = new(baseSchema,
                                        false,
                                        name,
                                        attribute.Description,
                                        attribute.Manual,
                                        attribute.SupportedModes,
                                        attribute.ExcludedModes);

            return command;
        }

        /// <summary>
        /// Resolves <see cref="CommandSchema"/>.
        /// </summary>
        public static BaseCommandSchema ResolveDynamic(Type type)
        {
            if (!KnownTypesHelpers.IsDynamicCommandType(type))
            {
                throw CommandResolverExceptions.InvalidCommandType(type);
            }

            return ResolveBase(type, new[] { OptionSchema.HelpOption });
        }

        /// <summary>
        /// Resolves <see cref="CommandSchema"/>.
        /// </summary>
        private static BaseCommandSchema ResolveBase(Type type, OptionSchema[] builtInOptions)
        {
            if (!KnownTypesHelpers.IsNormalOrDynamicCommandType(type))
            {
                throw CommandResolverExceptions.InvalidCommandType(type);
            }

            ParameterSchema[] parameters = type.GetProperties()
                                               .Select(ParameterSchemaResolver.TryResolve)
                                               .Where(p => p is not null)
                                               .OrderBy(p => p!.Order)
                                               .ToArray()!;

            OptionSchema[] options = type.GetProperties()
                                         .Select(OptionSchemaResolver.TryResolve!)
                                         .Where(o => o is not null)
                                         .Concat(builtInOptions)
                                         .ToArray()!;

            BaseCommandSchema command = new(type,
                                                       parameters,
                                                       options); ;

            ValidateParameters(command);
            ValidateOptions(command);

            return command;
        }

        #region Helpers
        private static void ValidateParameters(BaseCommandSchema command)
        {
            IGrouping<int, ParameterSchema>? duplicateOrderGroup = command.Parameters
                                                                          .GroupBy(a => a.Order)
                                                                          .FirstOrDefault(g => g.Count() > 1);

            if (duplicateOrderGroup is not null)
            {
                throw ParameterResolverExceptions.ParametersWithSameOrder(
                    command,
                    duplicateOrderGroup.Key,
                    duplicateOrderGroup.ToArray()
                );
            }

            IGrouping<string, ParameterSchema>? duplicateNameGroup = command.Parameters
                                                                            .Where(a => !string.IsNullOrWhiteSpace(a.Name))
                                                                            .GroupBy(a => a.Name!, StringComparer.Ordinal)
                                                                            .FirstOrDefault(g => g.Count() > 1);

            if (duplicateNameGroup is not null)
            {
                throw ParameterResolverExceptions.ParametersWithSameName(
                    command,
                    duplicateNameGroup.Key,
                    duplicateNameGroup.ToArray()
                );
            }

            ParameterSchema[] nonScalarParameters = command.Parameters
                                                           .Where(p => !p.Bindable.IsScalar)
                                                           .ToArray();

            if (nonScalarParameters.Length > 1)
            {
                throw ParameterResolverExceptions.TooManyNonScalarParameters(
                    command,
                    nonScalarParameters
                );
            }

            ParameterSchema? nonLastNonScalarParameter = command.Parameters
                                                                .OrderByDescending(a => a.Order)
                                                                .Skip(1)
                                                                .LastOrDefault(p => !p.Bindable.IsScalar);

            if (nonLastNonScalarParameter is not null)
            {
                throw ParameterResolverExceptions.NonLastNonScalarParameter(
                    command,
                    nonLastNonScalarParameter
                );
            }
        }

        private static void ValidateOptions(BaseCommandSchema command)
        {
            IGrouping<string, OptionSchema>? duplicateNameGroup = command.Options
                .Where(o => !string.IsNullOrWhiteSpace(o.Name))
                .GroupBy(o => o.Name!, StringComparer.Ordinal)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateNameGroup is not null)
            {
                throw OptionResolverExceptions.OptionsWithSameName(
                    command,
                    duplicateNameGroup.Key,
                    duplicateNameGroup.ToArray()
                );
            }

            IGrouping<char, OptionSchema>? duplicateShortNameGroup = command.Options
                .Where(o => o.ShortName is not null)
                .GroupBy(o => o.ShortName!.Value)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateShortNameGroup is not null)
            {
                throw OptionResolverExceptions.OptionsWithSameShortName(
                    command,
                    duplicateShortNameGroup.Key,
                    duplicateShortNameGroup.ToArray()
                );
            }

            IGrouping<string, OptionSchema>? duplicateEnvironmentVariableNameGroup = command.Options
                .Where(o => !string.IsNullOrWhiteSpace(o.FallbackVariableName))
                .GroupBy(o => o.FallbackVariableName!, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateEnvironmentVariableNameGroup is not null)
            {
                throw OptionResolverExceptions.OptionsWithSameEnvironmentVariableName(
                    command,
                    duplicateEnvironmentVariableNameGroup.Key,
                    duplicateEnvironmentVariableNameGroup.ToArray()
                );
            }
        }
        #endregion
    }
}