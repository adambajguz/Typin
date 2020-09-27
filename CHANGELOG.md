### v2.0 (xx-xxx-2020)

- Added custom DI containter support.
- Added `ShortcutDefinition` struct and user defined shortcuts configuration in `CliApplicationBuilder.UseInteractiveMode(...)`.
- Improvements in shortcuts handling.
- Renamed `[default]` directive to `[!]`.
- `[!]` directive is now required to execute user-defined default command. However, `-h`, `--help`, `--version` will still work without `[!]` directive.
- Improvements in `Ctrl+[Delete/Backspace/ArrowLeft/ArrowRight]` handling.
- Renamed `EnvironmentVariableName` to `FallbackVariableName`.
- Added `IOptionFallbackProvider`, as well as `EnvironmentVariableFallbackProvider` as default implementation of `IOptionFallbackProvider` and `EmptyFallbackProvider` that can be used to disable fallback.
- Command execution now heavily uses middleware pipeline (`ResolveCommandSchema` -> `HandleVersionOption` -> `ResolveCommandInstance` -> `HandleInteractiveDirective` -> `HandleHelpOption` -> `HandleInteractiveCommands` -> `ExecuteCommand`).
- Added `CliExecutionScope` and ensured that `Context.Input`, `Context.Command`, `Context.CommandDefaultValues`, `Context.CommandSchema`, and `Context.ExitCode` are reset to default values after middleware pipeline execution.
- Added `IHelpWriter`, renamed `HelpTextWriter` to `DefaultHelpWriter', and made `DefaultHelpWriter` a public class.

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
