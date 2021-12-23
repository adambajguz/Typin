namespace Typin.Help
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Extensions.Options;
    using Typin.Components;
    using Typin.Console;
    using Typin.Models.Schemas;
    using Typin.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Default implementation of <see cref="IHelpWriter"/> that prints help to console.
    /// </summary>
    public class DefaultHelpWriter : IHelpWriter
    {
        private const ConsoleColor TitleColor = ConsoleColor.Yellow;
        private const ConsoleColor VersionColor = ConsoleColor.Yellow;
        private const ConsoleColor HeaderColor = ConsoleColor.Magenta;
        private const ConsoleColor DirectiveNameColor = ConsoleColor.Green;
        private const ConsoleColor CommandNameColor = ConsoleColor.Cyan;
        private const ConsoleColor ParametersColor = ConsoleColor.White;
        private const ConsoleColor OptionsPlaceholderColor = ConsoleColor.White;
        private const ConsoleColor RequiredColor = ConsoleColor.Red;
        private const ConsoleColor ModeRestrictedColor = ConsoleColor.DarkYellow;
        private const ConsoleColor RequiredParameterNameColor = ConsoleColor.White;
        private const ConsoleColor OptionNameColor = ConsoleColor.White;
        private const ConsoleColor CommentColor = ConsoleColor.DarkGray;

        private readonly RootSchema _rootSchema;
        private readonly CliContext? _context;
        private readonly IOptionsMonitor<ApplicationMetadata> _metadata;
        private readonly IComponentProvider _componentProvider;
        private readonly IConsole _console;

        private int _column;
        private int _row;

        private bool IsEmpty => _column == 0 && _row == 0;

        /// <summary>
        /// Initializes an instance of <see cref="DefaultHelpWriter"/>.
        /// </summary>
        public DefaultHelpWriter(IRootSchemaAccessor rootSchemaAccessor,
                                 ICliContextAccessor cliContextAccessor,
                                 IOptionsMonitor<ApplicationMetadata> metadata,
                                 IComponentProvider componentProvider,
                                 IConsole console)
        {
            _rootSchema = rootSchemaAccessor.RootSchema;
            _context = cliContextAccessor.CliContext;
            _console = console;
            _componentProvider = componentProvider;
            _metadata = metadata;
        }

        /// <inheritdoc/>
        public void Write()
        {
            if (_context is not null)
            {
                Write(_context.Command.Schema, _context.Command.DefaultValues);
            }
        }

        /// <inheritdoc/>
        public void Write(ICommandSchema command,
                          IReadOnlyDictionary<IArgumentSchema, object?> defaultValues)
        {
            IEnumerable<ICommandSchema> childCommands = _rootSchema.GetChildCommands(command.Name)
                                                                  .OrderBy(x => x.Name);

            _console.ResetColor();
            _console.ForegroundColor = ConsoleColor.Gray;

            if (command.IsDefault)
            {
                WriteApplicationInfo();
            }

            WriteCommandDescription(command);
            WriteCommandUsage(_rootSchema.Directives, command, childCommands);
            WriteCommandParameters(command);
            WriteCommandOptions(command, defaultValues);
            WriteModeRestrictionsManual(command);
            WriteCommandChildren(command, childCommands);
            WriteDirectivesManual(_rootSchema.Directives);
            WriteCommandManual(command);

            WriteLine();
        }

        #region Console Output Helpers
        private void Write(char value)
        {
            _console.Output.Write(value);
            _column++;
        }

        private void Write(string value)
        {
            _console.Output.Write(value);
            _column += value.Length;
        }

        private void Write(ConsoleColor foregroundColor, string value)
        {
            _console.Output.WithForegroundColor(foregroundColor, (output) => Write(value));
        }

        private void WriteLine()
        {
            _console.Output.WriteLine();
            _column = 0;
            _row++;
        }

        private void WriteVerticalMargin(int size = 1)
        {
            for (int i = 0; i < size; i++)
            {
                WriteLine();
            }
        }

        private void WriteHorizontalMargin(int size = 2)
        {
            Write(new string(' ', size));
        }

        private void WriteColumnMargin(int columnSize = 24, int offsetSize = 2)
        {
            if (_column + offsetSize < columnSize)
            {
                WriteHorizontalMargin(columnSize - _column);
            }
            else
            {
                WriteHorizontalMargin(offsetSize);
            }
        }

        private void WriteHeader(string text)
        {
            Write(HeaderColor, text.ToUpperInvariant());
            WriteLine();
        }
        #endregion

        #region Application Info
        private void WriteApplicationInfo()
        {
            ApplicationMetadata metadata = _metadata.CurrentValue;

            // Title and version
            Write(TitleColor, metadata.Title ?? "<unknown>");
            Write(' ');
            Write(VersionColor, metadata.VersionText ?? "<unknown>");
            WriteLine();

            // Description
            if (!string.IsNullOrWhiteSpace(metadata.Description))
            {
                WriteHorizontalMargin();
                Write(metadata.Description);
                WriteLine();
            }
        }
        #endregion

        #region Mode restrictions
        private void WriteModeRestrictionsManual(ICommandSchema command)
        {
            //IReadOnlyCollection<Type> modesInApplication = _componentProvider.Get<ICliMode>();

            //if (modesInApplication.Count <= 1)
            //{
            //    return;
            //}

            //if (!IsEmpty)
            //{
            //    WriteVerticalMargin();
            //}

            //WriteHeader("Supported modes");

            //IEnumerable<Type> commandModes = (command.SupportedModes?.Count ?? 0) > 0 ? command.SupportedModes! : modesInApplication;

            //if ((command.ExcludedModes?.Count ?? 0) > 0)
            //{
            //    commandModes = commandModes.Except(command.ExcludedModes!);
            //}

            //foreach (Type mode in commandModes)
            //{
            //    WriteHorizontalMargin();
            //    Write(ModeRestrictedColor, mode.FullName ?? mode.Name);
            //    WriteLine();
            //}
            //WriteLine();

            //Write(CommentColor, "TIP: Commands and directives marked with ");
            //Write(ModeRestrictedColor, "@");
            //Write(CommentColor, " cannot be executed in every mode in the app.");
            //WriteLine();
        }
        #endregion

        #region Directives
        private void WriteDirectivesManual(IReadOnlyDictionary<string, DirectiveSchema> directives)
        {
            if (directives.Count == 0)
            {
                return;
            }

            if (!IsEmpty)
            {
                WriteVerticalMargin();
            }

            WriteHeader("Directives");

            foreach (KeyValuePair<string, DirectiveSchema> directive in directives.OrderBy(x => x.Value.Name))
            {
                DirectiveSchema schema = directive.Value;

                // Name
                if (schema.HasModeRestrictions())
                {
                    Write(ModeRestrictedColor, "@");
                    WriteHorizontalMargin(1);
                }
                else
                {
                    WriteHorizontalMargin();
                }

                Write(DirectiveNameColor, "[");
                Write(DirectiveNameColor, directive.Key);
                Write(DirectiveNameColor, "]");
                WriteDirectiveDescription(schema);
                WriteLine();
            }
        }

        private void WriteDirectiveDescription(DirectiveSchema directive)
        {
            if (string.IsNullOrWhiteSpace(directive.Description))
            {
                return;
            }

            WriteColumnMargin();
            Write(directive.Description);
        }
        #endregion

        #region Command
        private void WriteCommandDescription(ICommandSchema command)
        {
            if (string.IsNullOrWhiteSpace(command.Description))
            {
                return;
            }

            if (!IsEmpty)
            {
                WriteVerticalMargin();
            }

            WriteHeader("Description");

            WriteHorizontalMargin();
            Write(command.Description);
            WriteLine();
        }

        private void WriteCommandManual(ICommandSchema command)
        {
            //if (string.IsNullOrWhiteSpace(command.Manual))
            //{
            //    return;
            //}

            //if (!IsEmpty)
            //{
            //    WriteVerticalMargin();
            //}

            //WriteHeader("Manual");
            //WriteHorizontalMargin();

            //string text = TextUtils.ConvertTabsToSpaces(command.Manual);
            //text = TextUtils.AdjustNewLines(text);

            //Write(text);

            //WriteLine();
        }

        private void WriteCommandUsage(IReadOnlyDictionary<string, DirectiveSchema> directives,
                                       ICommandSchema command,
                                       IEnumerable<ICommandSchema> childCommands)
        {
            if (!IsEmpty)
            {
                WriteVerticalMargin();
            }

            WriteHeader("Usage");

            // Exe name
            //if (command.HasModeRestrictions())
            //{
            //    Write(ModeRestrictedColor, "@");
            //    WriteHorizontalMargin(1);
            //}
            //else
            {
                WriteHorizontalMargin();
            }

            Write(CommentColor, _metadata.CurrentValue.ExecutableName ?? "<executable>");

            // Child command placeholder
            if (directives.Any())
            {
                Write(' ');
                Write(DirectiveNameColor, "[directives]");
            }

            // Command name
            if (!string.IsNullOrWhiteSpace(command.Name))
            {
                Write(' ');
                Write(CommandNameColor, command.Name);
            }

            // Child command placeholder
            if (childCommands.Any())
            {
                Write(' ');
                Write(CommandNameColor, "[command]");
            }

            // Parameters
            foreach (IParameterSchema parameter in command.Model.Parameters)
            {
                Write(' ');
                Write(parameter.Bindable.IsScalar ? $"<{parameter.Name}>" : $"<{parameter.Name}...>");
            }

            // Required options
            foreach (IOptionSchema option in command.Model.Options.Where(o => o.IsRequired))
            {
                Write(' ');
                Write(ParametersColor, !string.IsNullOrWhiteSpace(option.Name) ? $"--{option.Name}" : $"-{option.ShortName}");

                Write(' ');
                Write(option.Bindable.IsScalar ? "<value>" : "<values...>");
            }

            // Options placeholder
            Write(' ');
            Write(OptionsPlaceholderColor, "[options]");

            WriteLine();
        }

        private void WriteCommandParameters(ICommandSchema command)
        {
            if (!command.Model.Parameters.Any())
            {
                return;
            }

            if (!IsEmpty)
            {
                WriteVerticalMargin();
            }

            WriteHeader("Parameters");

            foreach (IParameterSchema parameter in command.Model.Parameters.OrderBy(p => p.Order))
            {
                Write(RequiredColor, "* ");
                Write(RequiredParameterNameColor, $"{parameter.Name}");

                WriteColumnMargin();

                // Description
                if (!string.IsNullOrWhiteSpace(parameter.Description))
                {
                    Write(parameter.Description);
                    Write(' ');
                }

                // Valid values
                var validValues = parameter.Bindable.GetValidValues();
                if (validValues.Count > 0)
                {
                    WriteValidValues(parameter.Bindable.IsScalar, validValues);
                }

                WriteLine();
            }
        }

        private void WriteCommandOptions(ICommandSchema command,
                                         IReadOnlyDictionary<IArgumentSchema, object?> argumentDefaultValues)
        {
            if (!IsEmpty)
            {
                WriteVerticalMargin();
            }

            WriteHeader("Options");

            foreach (IOptionSchema option in command.Model.Options.OrderByDescending(o => o.IsRequired))
            {
                if (option.IsRequired)
                {
                    Write(RequiredColor, "* ");
                }
                else
                {
                    WriteHorizontalMargin();
                }

                // Short name
                if (option.ShortName is not null)
                {
                    Write(OptionNameColor, $"-{option.ShortName}");
                }

                // Separator
                if (!string.IsNullOrWhiteSpace(option.Name) && option.ShortName is not null)
                {
                    Write('|');
                }

                // Name
                if (!string.IsNullOrWhiteSpace(option.Name))
                {
                    Write(OptionNameColor, $"--{option.Name}");
                }

                WriteColumnMargin();

                // Description
                if (!string.IsNullOrWhiteSpace(option.Description))
                {
                    Write(option.Description);
                    Write(' ');
                }

                // Valid values
                var validValues = option.Bindable.GetValidValues();
                if (validValues.Count > 0)
                {
                    WriteValidValues(option.Bindable.IsScalar, validValues);
                }

                // Environment variable
                if (!string.IsNullOrWhiteSpace(option.FallbackVariableName))
                {
                    Write($"Fallback variable: \"{option.FallbackVariableName}\".");
                    Write(' ');
                }

                // Default value
                if (!option.IsRequired)
                {
                    object? defaultValue = argumentDefaultValues.GetValueOrDefault(option);
                    string? defaultValueFormatted = FormatDefaultValue(defaultValue);
                    if (defaultValueFormatted is not null)
                    {
                        Write("(Default: ");
                        Write(defaultValueFormatted);
                        Write(')');
                    }
                }

                WriteLine();
            }
        }
        #endregion

        #region Command Children
        private void WriteCommandChildren(ICommandSchema command, IEnumerable<ICommandSchema> childCommands)
        {
            if (!childCommands.Any())
            {
                return;
            }

            if (!IsEmpty)
            {
                WriteVerticalMargin();
            }

            WriteHeader("Commands");

            foreach (ICommandSchema childCommand in childCommands)
            {
                string relativeCommandName = !string.IsNullOrWhiteSpace(command.Name)
                    ? childCommand.Name![command.Name.Length..].Trim()
                    : childCommand.Name!;

                // Name
                //if (childCommand.HasModeRestrictions())
                //{
                //    Write(ModeRestrictedColor, "@");
                //    WriteHorizontalMargin(1);
                //}
                //else
                {
                    WriteHorizontalMargin();
                }

                Write(CommandNameColor, relativeCommandName);

                // Description
                if (!string.IsNullOrWhiteSpace(childCommand.Description))
                {
                    WriteColumnMargin();
                    Write(childCommand.Description);
                }

                WriteLine();
            }

            // Child command help tip
            WriteVerticalMargin();
            Write(CommentColor, "TIP: You can run `");

            if (!string.IsNullOrWhiteSpace(command.Name))
            {
                Write(CommandNameColor, command.Name);
                Write(' ');
            }

            Write(CommandNameColor, "[command]");
            Write(' ');

            Write(OptionNameColor, "--help");

            Write(CommentColor, "` to show help on a specific command.");

            WriteLine();
        }
        #endregion

        #region Helpers
        private void WriteValidValues(bool isScalar, IReadOnlyList<string> values)
        {
            if (isScalar)
            {
                Write("(One of: ");
            }
            else
            {
                Write("(Array of: ");
            }

            for (int i = 0; i < values.Count; i++)
            {
                string value = values[i];

                Write(ConsoleColor.DarkGray, value.Quote());

                if (i != values.Count - 1)
                {
                    Write(", ");
                }
            }

            Write(") ");
        }

        private static string? FormatDefaultValue(object? defaultValue)
        {
            if (defaultValue is null)
            {
                return null;
            }

            // Enumerable
            if (defaultValue is not string && defaultValue is IEnumerable defaultValues)
            {
                Type elementType = defaultValues.GetType().TryGetEnumerableUnderlyingType() ?? typeof(object);

                // If the ToString() method is not overriden, the default value can't be formatted nicely
                if (!elementType.IsToStringOverriden())
                {
                    return null;
                }

                return defaultValues.Cast<object?>()
                                    .Where(o => o is not null)
                                    .Select(o => o!.ToFormattableString(CultureInfo.InvariantCulture).Quote())
                                    .JoinToString(' ');
            }
            // Non-enumerable
            else
            {
                // If the ToString() method is not overriden, the default value can't be formatted nicely
                if (!defaultValue.GetType().IsToStringOverriden())
                {
                    return null;
                }

                return defaultValue.ToFormattableString(CultureInfo.InvariantCulture).Quote();
            }
        }
        #endregion
    }
}