### v1.0 (XX-Aug-2020)

- Added interactive mode `CliInteractiveApplication` and interactive only commands.
- Added `ICliExceptionHandler` and `CliApplicationBuilder.UseExceptionHandler(...)`
- Added	`Manual` property in `CommandAttribute` that can be used to provide a long, extended description of a commmand.
- Added `CliContext` that can be injected to services and commands with DI.
- Added `WindowWidth`, `WindowHeight`, `BufferWidth`, and `BufferHeight` to `IConsole`.
- Added new demo apps and improved existing demo.
- Added `"Debugger attached to PID {processId}.` message after debugger attachment.
- Added benchmarks for multiple commands.
- Added startup message option with macros.
- Rewritten `RootSchema` with HashSet for faster execution, esspecially in interactive mode.
- Added tests of the command used in benchmarking to easily check if it executs correctly and won't cause banchmarking freezing.
- Improved code readability.
- Removed `CliApplicationBuilder.UseTypeActivator` and added Microsoft.Extensions.DependencyInjection
- Added support for middlewares.
- Added `TableUtils` and `TextUtils`.
- Added history and auto-completion in interactive mode.

### v0.1 (Aug-2020)

- Indirect fork from CliFx.
