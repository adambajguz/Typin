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
            if (!SchemasHelpers.IsCommandType(type))
                throw CommandResolverExceptions.InvalidCommandType(type);

            CommandAttribute attribute = type.GetCustomAttribute<CommandAttribute>()!;

            if (modeTypes != null)
            {
                if (attribute.SupportedModes != null && attribute.SupportedModes.Except(modeTypes).Any())
                {
                    throw CommandResolverExceptions.InvalidSupportedModesInCommand(type, attribute);
                }

                if (attribute.ExcludedModes != null && attribute.ExcludedModes.Except(modeTypes).Any())
                {
                    throw CommandResolverExceptions.InvalidExcludedModesInCommand(type, attribute);
                }
            }


            string? name = attribute.Name;

            CommandOptionSchema[] builtInOptions = string.IsNullOrWhiteSpace(name)
                ? new[] { CommandOptionSchema.HelpOption, CommandOptionSchema.VersionOption }
                : new[] { CommandOptionSchema.HelpOption };

            CommandParameterSchema?[] parameters = type.GetProperties()
                                                       .Select(CommandParameterSchemaResolver.TryResolve)
                                                       .Where(p => p != null)
                                                       .OrderBy(p => p!.Order)
                                                       .ToArray();

            CommandOptionSchema?[] options = type.GetProperties()
                                                 .Select(CommandOptionSchemaResolver.TryResolve)
                                                 .Where(o => o != null)
                                                 .Concat(builtInOptions)
                                                 .ToArray();

            CommandSchema command = new CommandSchema(
                type,
                name,
                attribute.Description,
                attribute.Manual,
                attribute.SupportedModes,
                attribute.ExcludedModes,
                parameters!,
                options!
            );

            ValidateParameters(command);
            ValidateOptions(command);

            return command;
        }

        #region Helpers
        private static void ValidateParameters(CommandSchema command)
        {
            IGrouping<int, CommandParameterSchema>? duplicateOrderGroup = command.Parameters
                                                                                 .GroupBy(a => a.Order)
                                                                                 .FirstOrDefault(g => g.Count() > 1);

            if (duplicateOrderGroup != null)
            {
                throw ParameterResolverExceptions.ParametersWithSameOrder(
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
                throw ParameterResolverExceptions.ParametersWithSameName(
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
                throw ParameterResolverExceptions.TooManyNonScalarParameters(
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
                throw ParameterResolverExceptions.NonLastNonScalarParameter(
                    command,
                    nonLastNonScalarParameter
                );
            }
        }

        private static void ValidateOptions(CommandSchema command)
        {
            IGrouping<string, CommandOptionSchema>? duplicateNameGroup = command.Options
                .Where(o => !string.IsNullOrWhiteSpace(o.Name))
                .GroupBy(o => o.Name!, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateNameGroup != null)
            {
                throw OptionResolverExceptions.OptionsWithSameName(
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
                throw OptionResolverExceptions.OptionsWithSameShortName(
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