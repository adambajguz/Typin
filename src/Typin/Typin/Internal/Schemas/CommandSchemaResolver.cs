namespace Typin.Internal.Schemas
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Exceptions.Mode;
    using Typin.Exceptions.Resolvers.CommandResolver;
    using Typin.Exceptions.Resolvers.OptionResolver;
    using Typin.Exceptions.Resolvers.ParameterResolver;
    using Typin.Models.Binding;
    using Typin.Models.Schemas;
    using Typin.Schemas;

    /// <summary>
    /// Resolves an instance of <see cref="CommandSchema"/>.
    /// </summary>
    internal static class CommandSchemaResolver
    {
        /// <summary>
        /// Resolves <see cref="CommandSchema"/>.
        /// </summary>
        public static CommandSchema Resolve(Type type)
        {
            if (!KnownTypesHelpers.IsCommandOrTemplateType(type))
            {
                throw new InvalidCommandException(type);
            }

            CommandAttribute attribute = type.GetCustomAttribute<CommandAttribute>()!;

            string? name = attribute.Name; //TODO: add DynamicCommandSchema beacuse dynamic commands can only be descirbed by BindableModelSchema

            IParameterSchema[] parameters = ResolveParameters(type);
            IOptionSchema[] options = ResoveOptions(type);

            if (attribute.SupportedModes is not null)
            {
                Type? invalidMode = attribute.SupportedModes.FirstOrDefault(x => !KnownTypesHelpers.IsCliModeType(x));

                if (invalidMode is not null)
                {
                    throw new InvalidModeException(invalidMode);
                }
            }

            if (attribute.ExcludedModes is not null)
            {
                Type? invalidMode = attribute.ExcludedModes.FirstOrDefault(x => !KnownTypesHelpers.IsCliModeType(x));

                if (invalidMode is not null)
                {
                    throw new InvalidModeException(invalidMode);
                }
            }

            CommandSchema command = new(type,
                                        false,
                                        name,
                                        attribute.Description,
                                        attribute.Manual,
                                        attribute.SupportedModes,
                                        attribute.ExcludedModes,
                                        parameters,
                                        options);

            ValidateParameters(command);
            ValidateOptions(command);

            return command;
        }

        /// <summary>
        /// Resolves <see cref="CommandSchema"/>.
        /// </summary>
        public static ICommandTemplateSchema ResolveTemplate(Type type)
        {
            if (!KnownTypesHelpers.IsCommandTemplateType(type))
            {
                throw new InvalidCommandException(type);
            }

            IParameterSchema[] parameters = ResolveParameters(type);
            IOptionSchema[] options = ResoveOptions(type);

            CommandTemplateSchema commandTempalte = new(type,
                                                        parameters,
                                                        options);

            ValidateParameters(commandTempalte);
            ValidateOptions(commandTempalte);

            return commandTempalte;
        }

        #region Helpers
        private static IParameterSchema[] ResolveParameters(Type type)
        {
            return type.GetProperties()
                       .Select(ParameterSchemaResolver.TryResolve)
                       .Where(p => p is not null)
                       .OrderBy(p => p!.Order)
                       .ToArray()!;
        }

        private static IOptionSchema[] ResoveOptions(Type type)
        {
            return type.GetProperties()
                       .Select(OptionSchemaResolver.TryResolve!)
                       .Where(o => o is not null)
                       .ToArray()!;
        }

        private static void ValidateParameters(IModelSchema command)
        {
            IGrouping<int, IParameterSchema>? duplicateOrderGroup = command.Parameters
                .GroupBy(a => a.Order)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateOrderGroup is not null)
            {
                throw new ParameterDuplicateByOrderException(
                    command,
                    duplicateOrderGroup.Key,
                    duplicateOrderGroup.ToArray()
                );
            }

            IGrouping<string, IParameterSchema>? duplicateNameGroup = command.Parameters
                .Where(a => !string.IsNullOrWhiteSpace(a.Name))
                .GroupBy(a => a.Name!, StringComparer.Ordinal)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateNameGroup is not null)
            {
                throw new ParameterDuplicateByNameException(
                    command,
                    duplicateNameGroup.Key,
                    duplicateNameGroup.ToArray()
                );
            }

            IParameterSchema[] nonScalarParameters = command.Parameters
                .Where(p => !p.Bindable.IsScalar)
                .ToArray();

            if (nonScalarParameters.Length > 1)
            {
                throw new TooManyNonScalarParametersException(
                    command,
                    nonScalarParameters
                );
            }

            IParameterSchema? nonLastNonScalarParameter = command.Parameters
                .OrderByDescending(a => a.Order)
                .Skip(1)
                .LastOrDefault(p => !p.Bindable.IsScalar);

            if (nonLastNonScalarParameter is not null)
            {
                throw new NonScalarParametersMustHaveHighestOrderException(
                    command,
                    nonLastNonScalarParameter
                );
            }
        }

        private static void ValidateOptions(IModelSchema command)
        {
            IGrouping<string, IOptionSchema>? duplicateNameGroup = command.Options
                .Where(o => !string.IsNullOrWhiteSpace(o.Name))
                .GroupBy(o => o.Name!, StringComparer.Ordinal)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateNameGroup is not null)
            {
                throw new OptionDuplicateByNameException(
                    command,
                    duplicateNameGroup.Key,
                    duplicateNameGroup.ToArray()
                );
            }

            IGrouping<char, IOptionSchema>? duplicateShortNameGroup = command.Options
                .Where(o => o.ShortName is not null)
                .GroupBy(o => o.ShortName!.Value)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateShortNameGroup is not null)
            {
                throw new OptionDuplicateByShortNameException(
                    command,
                    duplicateShortNameGroup.Key,
                    duplicateShortNameGroup.ToArray()
                );
            }
        }
        #endregion
    }
}