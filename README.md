# Typin

<p align="center">

[![Build](https://github.com/adambajguz/Typin/workflows/Typin-CI/badge.svg?branch=master&v=15)](https://github.com/adambajguz/Typin/actions)
[![Coverage](https://codecov.io/gh/adambajguz/Typin/branch/master/graph/badge.svg?v=15)](https://codecov.io/gh/adambajguz/Typin)
[![Version](https://img.shields.io/nuget/v/Typin.svg?label=NuGet)](https://nuget.org/packages/Typin)
[![Downloads of Typin](https://img.shields.io/nuget/dt/Typin.svg?label=Typin)](https://nuget.org/packages/Typin)
[![Downloads of Typin.Core](https://img.shields.io/nuget/dt/Typin.Core.svg?label=Typin.Core)](https://nuget.org/packages/Typin.Core)

</p>

**See [TypinExamples](https://adambajguz.github.io/Typin/) website for interactive examples.**

<table>
 <tr>
    <td>
      <p align="center">
        <img src="https://raw.githubusercontent.com/adambajguz/Typin/master/.img/typin-logo-256px.png" width="75%">
      </p>
    </td>
    <td>

<b>Table of contents</b>

- [Typin](#typin)
  - [Typin roots in CliFx](#typin-roots-in-clifx)
  - [Features](#features)
  - [Installing Typin](#installing-typin)
  - [Getting started and documentation](#getting-started-and-documentation)
  - [Screenshots](#screenshots)
  - [Benchmarks](#benchmarks)
  
  </td>
 </tr>
</table>



**Typin** is a simple to use, ASP.NET Core inspired framework for building both interactive command line applications and command line tools (direct mode). However, it is not limited to direct and interactive modes, because you can create your own modes.

> **Etymology:** Typin is made out of "Typ" for "Type" and "in" for "interactively". It's pronounced as "Ty pin".

Typin is not just a parser but a complete application framework. Its main goal is to provide an enjoyable, similar to ASP.NET Core, development experience when building command line applications. Its primary goal is to completely take over the user input layer, letting you forget about the infrastructure and instead focus on writing your application.

## Typin roots in CliFx

Typin is build based on the source code of [CliFx](https://github.com/Tyrrrz/CliFx), but it wants to be a ASP.NET Core for CLI like [Cocona](https://github.com/mayuki/Cocona) but faster. It has many additional features compared to CliFx:

- [Interactive mode](https://github.com/adambajguz/Typin/wiki/Interactive-mode) with auto-completion, parameter escaping with `"`, and support for user-defined shortcuts,
- [Middleware pipeline](https://github.com/adambajguz/Typin/wiki/Middleware-pipeline),
- [Custom directives](https://github.com/adambajguz/Typin/wiki/Defining-custom-directives) that can be used as either flags (`IDirective`) or dynamic pipeline extensions (`IPipelinedDirective`),
- [IOptionFallbackProvider](https://github.com/adambajguz/Typin/wiki/Option-fallback) for custom fallback providers instead of only environment variable fallback,
- [Build-in DI support](https://github.com/adambajguz/Typin/wiki/Dependency-injection) with `Microsoft.Extensions.DependencyInjection` that is used across entire framework,
- Build-in options support with `Microsoft.Extensions.Options`,
- Ability to modify [exception handling](https://github.com/adambajguz/Typin/wiki/Exception-handling) with one or more exception handlers,
- Ability to execute commands from other commands or services with `ICliCommandExecutor` (NOT RECOMMENDED, except for custom CLI mode classes),
- DI injectable `ICliContext` with lots of useful data,
- Manual property in `CommandAttribute` that can be used to provide a long, extended description of a command,
- Custom help writer.
- Custom modes support and application lifetime management.
- Startup message color personalization through a callback method.
- Console IO wrapper classes (`StandardStreamReader` and `StandardStreamWriter`) and IO interfaces.
- Logging with `Microsoft.Extensions.Logging`.
- Optional option and parameter names by automatically generated kebab case name.
- Better char parsing: support for the following escape sequences: '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v', '\\\\', and Unicode escape e.g. \\u006A).
- Native support for `Half`, `DateOnly`, and `TimeOnly`.
- Validation can be easily added with [FluentValidation](https://github.com/FluentValidation/FluentValidation) and [a middleware](https://github.com/adambajguz/Typin/blob/master/src/TypinExamples/Examples/TypinExamples.Validation/Middleware/FluentValidationMiddleware.cs).
- Console input/output targeted extensions through `IStandardInput`, `IStandardOuput`, `IStandardError`, `IStandardOutputAndError`, `IStandardRedirectableConsoleStream`, `StandardStreamReader`, `StandardStreamWriter`.

Overall, Typin is a framework that is much more flexible and rich with both features and metadata about defined commands etc.

> See [CHANGELOG.md](https://github.com/adambajguz/Typin/blob/master/CHANGELOG.md) for a complete list of changes.

> Also see [WIKI: Roadmap and support](https://github.com/adambajguz/Typin/wiki/Roadmap-and-support) for more info about future and support.

## Features

- Complete application framework
- Argument (options and parameters) parser.
- Requires minimal amount of code to get started.
- Configuration via attributes.
- Handles conversions to various types, including custom types.
- Similarly to ASP.NET Core, relies on dependency injection, thus is very extensible.
- Supports multi-level command hierarchies.
- Supports interactive mode and user defined modes.
- Intuitive auto-completion (Tab / Shift + Tab) in interactive mode.
- Intuitive command history (Up and Down arrows) in interactive mode, accessible also from user code.
- Exposes raw input, output, error streams to handle binary data.
- Allows graceful command cancellation.
- Prints errors and routes exit codes on exceptions.
- Provides comprehensive and colorful auto-generated help text.
- Highly testable and easy to debug.
- Automatic generation of option and parameter names by transforming property name with kebab-case formatter.
- Targets .NET 5.0 and .NET 6.0.
- Uses `Microsoft.Extensions.DependencyInjection`. `Microsoft.Extensions.Logging.Debug` and `Microsoft.Extensions.Options` but no other non essential dependencies.

## Installing Typin

You should install [Typin with NuGet](https://www.nuget.org/packages/Typin):

    Install-Package Typin
    
Or via the .NET Core command line interface:

    dotnet add package Typin

Both commands will download and install Typin with all required dependencies.

### Typin.Core

If you need only API interfaces, you can install [TypinCore with NuGet](https://www.nuget.org/packages/Typin).

    Install-Package Typin.Core
    
Or via the .NET Core command line interface:

    dotnet add package Typin.Core

Both commands will download and install Typin.Core with all required dependencies.

## Getting started and Documentation

```c#
public static class Program
{
    public static async Task<int> Main() =>
        await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .Build()
            .RunAsync();
}

[Command]
public class HelloWorldCommand : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        await console.Output.WriteLineAsync("Hello world!");
    }
}
```

See [wiki](https://github.com/adambajguz/Typin/wiki) for detailed instructions and documentation.

## Screenshots

![help screen](.screenshots/help.png)

## Benchmarks

Here's how Typin's execution overhead compares to that of other libraries (single command comparison) and with increasing number of commands.

### Typin 3.1

```ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
```

|                               Method |         Mean |      Error |      StdDev | Ratio | Rank |
|------------------------------------- |-------------:|-----------:|------------:|------:|-----:|
|                    CommandLineParser |     1.810 us |  0.0283 us |   0.0265 us | 0.009 |    1 |
|                                CliFx |    70.937 us |  0.8013 us |   0.7103 us | 0.373 |    2 |
|                                Clipr |    81.382 us |  1.3975 us |   1.3725 us | 0.428 |    3 |
| McMaster.Extensions.CommandLineUtils |    88.881 us |  1.0713 us |   1.0021 us | 0.468 |    4 |
|                                Typin |   190.249 us |  0.3856 us |   0.3220 us | 1.000 |    5 |
|                   System.CommandLine |   278.502 us |  5.5451 us |   5.9332 us | 1.462 |    6 |
|                            PowerArgs |   300.629 us |  1.3090 us |   1.1604 us | 1.579 |    7 |
|                               Cocona | 1,283.562 us | 88.6368 us | 244.1316 us | 6.492 |    8 |


|                Method |      Mean |    Error |   StdDev | Ratio | Rank |
|---------------------- |----------:|---------:|---------:|------:|-----:|
|   'CliFx - 1 command' |  68.67 us | 0.207 us | 0.173 us |  0.36 |    1 |
|  'CliFx - 2 commands' |  77.32 us | 0.577 us | 0.512 us |  0.40 |    2 |
|  'CliFx - 5 commands' |  98.64 us | 0.260 us | 0.230 us |  0.51 |    3 |
| 'CliFx - 10 commands' | 135.11 us | 0.317 us | 0.297 us |  0.70 |    4 |
|   'Typin - 1 command' | 192.11 us | 0.662 us | 0.553 us |  1.00 |    5 |
|  'Typin - 2 commands' | 202.54 us | 0.851 us | 0.754 us |  1.05 |    6 |
| 'CliFx - 20 commands' | 231.62 us | 0.502 us | 0.445 us |  1.21 |    7 |
|  'Typin - 5 commands' | 237.19 us | 0.388 us | 0.363 us |  1.23 |    8 |
| 'Typin - 10 commands' | 298.32 us | 2.848 us | 2.378 us |  1.55 |    9 |
| 'Typin - 20 commands' | 440.23 us | 0.773 us | 0.646 us |  2.29 |   10 |

### Typin <= 2.1.1

```ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.19041
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.402
  [Host]     : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
  DefaultJob : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
```

|                               Method |         Mean |     Error |    StdDev | Ratio | Rank |
|------------------------------------- |-------------:|----------:|----------:|------:|-----:|
|                    CommandLineParser |     2.489 us | 0.0481 us | 0.0573 us |  0.03 |    1 |
|                                CliFx |    51.513 us | 0.3411 us | 0.3024 us |  0.57 |    2 |
|                                Typin |    90.748 us | 0.4652 us | 0.4351 us |  1.00 |    3 |
| McMaster.Extensions.CommandLineUtils |   129.112 us | 1.5520 us | 1.3758 us |  1.42 |    4 |
|                                Clipr |   131.652 us | 2.8059 us | 4.1129 us |  1.47 |    4 |
|                   System.CommandLine |   198.114 us | 3.7021 us | 3.4630 us |  2.18 |    5 |
|                            PowerArgs |   257.859 us | 1.4766 us | 1.3812 us |  2.84 |    6 |
|                               Cocona |      1166 us | 7.3347 us | 6.1248 us | 12.86 |    7 |


|                Method |      Mean |    Error |   StdDev | Ratio | Rank |
|---------------------- |----------:|---------:|---------:|------:|-----:|
|   'CliFx - 1 command' |  51.15 us | 0.843 us | 0.788 us |  0.54 |    1 |
|  'CliFx - 2 commands' |  71.39 us | 1.375 us | 1.972 us |  0.76 |    2 |
|   'Typin - 1 command' |  94.46 us | 1.974 us | 2.027 us |  1.00 |    3 |
|  'Typin - 2 commands' | 118.90 us | 2.668 us | 4.811 us |  1.29 |    4 |
|  'CliFx - 5 commands' | 126.71 us | 1.908 us | 1.692 us |  1.34 |    5 |
|  'Typin - 5 commands' | 180.16 us | 3.459 us | 3.701 us |  1.91 |    6 |
| 'CliFx - 10 commands' | 222.28 us | 3.079 us | 2.880 us |  2.35 |    7 |
| 'Typin - 10 commands' | 281.79 us | 4.679 us | 4.148 us |  2.99 |    8 |
| 'CliFx - 20 commands' | 454.07 us | 8.708 us | 8.942 us |  4.81 |    9 |
| 'Typin - 20 commands' | 519.70 us | 6.735 us | 6.300 us |  5.50 |   10 |

Legends:
  * Mean    : Arithmetic mean of all measurements
  * Error   : Half of 99.9% confidence interval
  * StdDev  : Standard deviation of all measurements
  * Ratio   : Mean of the ratio distribution ([Current]/[Baseline])
  * RatioSD : Standard deviation of the ratio distribution ([Current]/[Baseline])
  * Rank    : Relative position of current benchmark mean among all benchmarks (Arabic style)
  * 1 us    : 1 Microsecond (0.000001 sec)
