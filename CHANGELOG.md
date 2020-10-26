### v2.2.0 (xx-Oct-2020)

- Added `CliApplication.RunAsync` with string command line.
- Advanced interactive input is disabled when input is redirected.
- Renamed `normal mode` to `direct mode`.
- Added `CliModeSwitcher` class and `CliContext.ModeSwitcher`.
- Annotated `CliContext.IsInteractiveMode` as obsolete. Use `CliContext.ModeSwitcher.Current` instead.` IsInteractiveMode` will be removed in Typin 3.0.

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
- Removed `CliApplicationBuilder.UseTypeActivator` and added Microsoft.Extensions.DependencyInjection
- Added support for middlewares.
- Added `TableUtils` and `TextUtils`.
- Added history and auto-completion in interactive mode.

### v0.1 (08-Aug-2020)

- Indirect fork from CliFx.
