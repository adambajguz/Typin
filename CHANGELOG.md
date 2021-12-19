### v3.1.0 (19-Dec-2021)

 - .NET 6 support.
 - Fixed `StackTraceParser`: add a filter for `--- End of stack trace from previous location ---` and `--- End of stack trace from previous location where exception was thrown ---`.
 
### v3.0.4 (06-Apr-2021)

 - Fixed issue #208: interactive mode executes the command line arguments at every new commands.

### v3.1.0 (xx-Apr-2021)

 - Added `InteractiveModeOptions.Prompt`, `InteractiveModeOptions.SetDefaultPrompt()` and `InteractiveModeOptions.SetPrompt()` for specifying custom prompt template. `PromptForeground` and `ScopeForeground` can still be used to configure foreground without changing prompt template.
 - Added `BindableProperyInfo` and `ArgumentSchema.BindableProperty`, as well as marked `ArgumentSchema.Property`, `ArgumentSchema.IsScalar`, and `ArgumentSchema.GetValidValues()` obsolete - will be removed in Typin 4.0.
 - Removed `IDisposable` from `CliContext`.
 - Faster `Guid` binding by explicit `Guid.Parse()` call (added `Guid` to `ArgumentBinder.PrimitiveConverters`).
 - Changed default values format in help - now in round brackets.
 - `ArgumentSchema.IsScalar` and `ArgumentSchema.GetValidValues()` are now optimized with a simple cache (backing field).
 - Fixed dependency injection `IDisposable` anti-pattern in `IConsole` - `IDisposable` is no longer present in `IConsole`. If you wish to use it, implement it in `IConsole` implementation.
 - Fixed console not being disposed when stopping the application.
 - Fixed invalid help text: `Environment variable:` instead of `Fallback variable:`.
 - Fixed showing choices for non-scalar nullable and non-nullable enum arguments.

### v3.0.3 (06-Apr-2021)

 - Fixed `DefaultDirective` (`[!]`) executes default command - unable to execute scoped command without parameters, e.g., `[>] books\r [!]\r` was executing default command.
 - Fixed `DefaultDirective` behavior (more consistent with direct mode): it WILL NOT (was: WILL) force default command execution when input contains default commmand parameter values equal to command/subcommand name.

### v3.0.2 (05-Apr-2021)

 - Fixed `CliApplicationBuilder.UseHelpWriter<>` - it was using `UseOptionFallbackProvider(Type)` instead of `UseHelpWriter(Type)`.
 
### v3.0.1 (22-Mar-2021)

 - Fixed Ctrl+C in interactive mode.
 - .NET 5/C# 9 related refactor.

### v3.0.0 (23-Feb-2021)

- Added `Typin.Core` library.
- Core middleware execution order has changed: `ResolveCommandSchemaAndInstance` -> `InitializeDirectives` -> `ExecuteDirectivesSubpipeline` -> [Directives subpipeline] -> `HandleSpecialOptions` -> `BindInput` -> [User middlewares] -> `ExecuteCommand`.
- Renamed `normal mode` to `direct mode`, and added support for custom modes.
- It is now possible to register multiple exception handleres to handle different exceptions in app.
- Major API and command execution changes: a) added `ICliApplicationLifetime`, `ICliMode`, `ICliCommandExecutor`, `ICliApplicationLifetime`, `DirectMode`, `InteractiveMode`, `IPipelinedDirective`, and more; b) removed `InteractiveCliApplication`.
- Removed `HandleInteractiveDirective` and `HandleInteractiveCommands` middlewares.
- Replaced `IsInteractiveModeOnly` with `SupportedModes` and `ExcludedModes`.
- Added support for options with no name by automatic conversion of property names.
- Added native support for .NET 5.0 (including usage of `init` instead of `get`).
- Added `Typin.Console.IO` namespace with `IStandardInput`, `IStandardOuput`, `IStandardError`, `IStandardOutputAndError`, `IStandardRedirectableConsoleStream`, `StandardStreamReader`, `StandardStreamWriter`.
- Rewritten `Typin.Core.Console.ConsoleExtensions` to target `StandardStreamWriter`.
- User middlewares are now executed after command instance creation.
- Middleware types collection in `ApplicationConfiguration` order was reversed.
- Merged `HandleVersionOption` and `HandleHelpOption` into one middleware named `HandleSpecialOptions`.
- Removed unnecessary casts to `CliContext` from `ICliContext`.
- Removed `IDirective.ContinueExecution`, modified `IDirective`, and added `IPipelinedDirective`.
- `CommandPipelineHandlerDelegate` now uses `ValueTask` instead of a `Task`.
- Added logging with `Microsoft.Extensions.Logging` (default logger is `DebugLogger`).
- Added `IConsole.ReadKeyAsync()`.
- Option name with 3 characters is no longer treated as option alias (e.g., `--h` is not `-h`).
- Option name and short name must start with letter (previously not start with digit).
- Parameter names are generated using `StringExtensions.ToHyphenCase()` instead of `string.ToLowerInvariant()`.
- Option attributes are validated in ctor, and appropiate exception is thrown without the need of resolving RootSchema.
- Added `TextUtils.UnescapeChar()` and a support for the following escape sequences: '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v', '\\\\', and Unicode escape e.g. \\u006A) during char parsing.
- Added `CliApplication.RunAsync` with string command line and replaced `IReadOnlyList<string>` with `IEnumerable<string>`.
- Advanced interactive input is disabled when input is redirected.
- Added `IRootSchemaAccessor` and `IEnvironmentVariablesAccessor` singleton services;
- Added `ExceptionFormatter` utility and used it as a default exception printer in `DefaultExceptionHandler`.
- `TableUtils` refactory and fix for proper handling of empty collection.
- `[!]` directive is now required only to execute command without parameters and options.
- Added startup message color personalization, and replaced string formating based on macros with `Func<ApplicationMetadata, string>` and `Action<ApplicationMetadata, IConsole>`.
- Fixed case-sensitivity of command and option names (always case-sesitive).
- Fixed interactive mode autocompletion results (fo 'column chan' TAB TAB result was 'column column change-range' instead of 'column change-range').

### v2.1.1 (18-Oct-2020)

- Fixed `CommandOptionInput.IsOptionAlias` bug.
- It is no possible to scope to `cmd` command even if there is only `cmd sub` in application.
- Added `CommandInput.Arguments`.
- Added `RootSchema.IsCommandOrSubcommandPart()`.
- `[>]` is now not resseting the scope when no name after `[>]`.

### v2.1 (17-Oct-2020)

- Schemas resolving improvements
- Added support for strings with spaces by surrounding with `"` in interactive mode (to escape `"` type `""`) with a custom command line splitter that works in both interactive and direct modes.
- Fixed negative numbers handling by forbidding options starting from digit. Options must have a name starting from char other than digit, while short name must not be a digit.
- Auto-completion bug fixes.

### v2.0 (02-Oct-2020)

- Added preview of custom DI containter support (`CliApplicationBuilder.UseServiceProviderFactory` and `CliApplicationBuilder.ConfigureContainer`).
- Added `ShortcutDefinition` struct and user defined shortcuts configuration in `CliApplicationBuilder.UseInteractiveMode(...)`.
- Improvements in shortcuts handling.
- Renamed `[default]` directive to `[!]`.
- `[!]` directive is now required to execute user-defined default command. However, `-h`, `--help`, `--version` will still work without `[!]` directive.
- Improvements in `Ctrl+[Delete/Backspace/ArrowLeft/ArrowRight]` handling.
- Renamed `EnvironmentVariableName` to `FallbackVariableName`.
- Added `IOptionFallbackProvider`, as well as `EnvironmentVariableFallbackProvider` as default implementation of `IOptionFallbackProvider` and `EmptyFallbackProvider` that can be used to disable fallback.
- Command execution now heavily uses middleware pipeline (`ResolveCommandSchema` -> `HandleVersionOption` -> `ResolveCommandInstance` -> `HandleInteractiveDirective` -> `HandleHelpOption` -> `HandleInteractiveCommands` -> `ExecuteCommand`).
- Added `CliExecutionScope` and ensured that `Context.Input`, `Context.Command`, `Context.CommandDefaultValues`, `Context.CommandSchema`, and `Context.ExitCode` are reset to default values after middleware pipeline execution.
- Added `IHelpWriter`, renamed `HelpTextWriter` to `DefaultHelpWriter`, and made `DefaultHelpWriter` a public class.
- Fixed `AddDirectivesFrom(Assembly directiveAssembly)` and `AddDirectivesFrom(IEnumerable<Assembly> directiveAssemblies)`.
- Removed partial classes.

### v1.0.1 (23-Aug-2020)

- Removed middleware delegate parameters.

### v1.0 (23-Aug-2020)

- Added interactive mode `CliInteractiveApplication` and interactive only commands.
- Added `ICliExceptionHandler` and `CliApplicationBuilder.UseExceptionHandler(...)`
- Added	`Manual` property in `CommandAttribute` that can be used to provide a long, extended description of a commmand.
- Added `CliContext` that can be injected to services and commands with DI.
- Added `ReadKey`, `SetCursorPosition`, `WindowWidth`, `WindowHeight`, `BufferWidth`, and `BufferHeight` to `IConsole`.
- Added new demo apps and improved existing demo.
- Added `Debugger attached to PID {processId}.` message after debugger attachment.
- Added benchmarks for multiple commands.
- Added startup message option with macros.
- Rewritten `RootSchema` with HashSet for faster execution, esspecially in interactive mode.
- Added tests of the command used in benchmarking to easily check if it executs correctly and won't cause banchmarking freezing.
- Improved code readability.
- Removed `CliApplicationBuilder.UseTypeActivator` and added `Microsoft.Extensions.DependencyInjection`
- Added support for middlewares.
- Added `TableUtils` and `TextUtils`.
- Added history and auto-completion in interactive mode.

### v0.1 (08-Aug-2020)

- Indirect fork from CliFx.
