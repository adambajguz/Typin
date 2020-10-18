# Typin

<p align="center">

[![Build](https://github.com/adambajguz/Typin/workflows/CI/badge.svg?branch=master)](https://github.com/adambajguz/Typin/actions)
[![Coverage](https://codecov.io/gh/adambajguz/Typin/branch/master/graph/badge.svg?v=10)](https://codecov.io/gh/adambajguz/Typin)
[![Version](https://img.shields.io/nuget/v/Typin.svg)](https://nuget.org/packages/Typin)
[![Downloads](https://img.shields.io/nuget/dt/Typin.svg)](https://nuget.org/packages/Typin)

</p>

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



**Typin** is a simple to use, ASP.NET Core inspired framework for building both interactive command line applications and command line tools.

> **Etymology:** Typin is made out of "Typ" for "Type" and "in" for "interactively". It's pronounced as "Type in".

Typin is not just a parser but a complete application framework. Its main goal is to provide an enjoyable, similar to ASP.NET Core, development experience when building command line applications. Its primary goal is to completely take over the user input layer, letting you forget about the infrastructure and instead focus on writing your application.

## Typin roots in CliFx

Typin is build based on the source code of [CliFx](https://github.com/Tyrrrz/CliFx), but it wants to be a ASP.NET Core for CLI like [Cocona](https://github.com/mayuki/Cocona) but faster. Typin has many additional functions compared to CliFx:

- [Interactive mode](https://github.com/adambajguz/Typin/wiki/Interactive-mode) with auto-completion, parameter escaping with `"`, and support for user-defined shortcuts,
- [Middleware pipeline](https://github.com/adambajguz/Typin/wiki/Middleware-pipeline),
- [Custom directives](https://github.com/adambajguz/Typin/wiki/Defining-custom-directives),
- [Build-in DI support](https://github.com/adambajguz/Typin/wiki/Dependency-injection) with `Microsoft.Extensions.DependencyInjection` that is used accross entire framework,
- [IOptionFallbackProvider](https://github.com/adambajguz/Typin/wiki/Option-fallback) for custom fallback providers instead of only environment variable fallback,
- Ability to modify [exception handling](https://github.com/adambajguz/Typin/wiki/Exception-handling) messages,
- DI injectable `ICliContext` with lots of useful data,
- Negative numbers handling,
- Manual property in `CommandAttribute` that can be used to provide a long, extended description of a commmand
- (more coming soon).

> See [CHANGELOG.md](https://github.com/adambajguz/Typin/blob/master/CHANGELOG.md) for a complete list of changes.

## Features

- Complete application framework, not just an argument parser
- Requires minimal amount of code to get started
- Configuration via attributes
- Handles conversions to various types, including custom types
- Supports multi-level command hierarchies
- Supports interactive mode
- Intuitive auto-completion (Tab / Shift + Tab) in interactive mode.
- Intuitive command history (Up and Down arrows) in interactive mode, accessible also from user code.
- Exposes raw input, output, error streams to handle binary data
- Allows graceful command cancellation
- Prints errors and routes exit codes on exceptions
- Provides comprehensive and colorful auto-generated help text
- Highly testable and easy to debug
- Comes with built-in analyzers to help catch common mistakes
- Targets .NET Standard 2.0+
- Uses `Microsoft.Extensions.DependencyInjection` but no other external dependencies

## Installing Typin

You should install [Typin with NuGet](https://www.nuget.org/packages/Typin):

    Install-Package Typin
    
Or via the .NET Core command line interface:

    dotnet add package Typin

Either commands, from Package Manager Console or .NET Core CLI, will download and install Typin and all required dependencies.

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

```ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.19041
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.402
  [Host]     : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
  DefaultJob : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
```

|                               Method |         Mean |     Error |    StdDev |       Median | Ratio | RatioSD | Rank |
|------------------------------------- |-------------:|----------:|----------:|-------------:|------:|--------:|-----:|
|                    CommandLineParser |     2.489 us | 0.0481 us | 0.0573 us |     2.500 us |  0.03 |    0.00 |    1 |
|                                CliFx |    51.513 us | 0.3411 us | 0.3024 us |    51.551 us |  0.57 |    0.00 |    2 |
|                                Typin |    90.748 us | 0.4652 us | 0.4351 us |    90.759 us |  1.00 |    0.00 |    3 |
| McMaster.Extensions.CommandLineUtils |   129.112 us | 1.5520 us | 1.3758 us |   128.436 us |  1.42 |    0.02 |    4 |
|                                Clipr |   131.652 us | 2.8059 us | 4.1129 us |   129.439 us |  1.47 |    0.05 |    4 |
|                   System.CommandLine |   198.114 us | 3.7021 us | 3.4630 us |   195.986 us |  2.18 |    0.04 |    5 |
|                            PowerArgs |   257.859 us | 1.4766 us | 1.3812 us |   258.043 us |  2.84 |    0.02 |    6 |
|                               Cocona |      1166 us | 7.3347 us | 6.1248 us |      1167 us | 12.86 |    0.10 |    7 |


|                Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|---------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|   'CliFx - 1 command' |  51.15 us | 0.843 us | 0.788 us |  0.54 |    0.01 |    1 |
|  'CliFx - 2 commands' |  71.39 us | 1.375 us | 1.972 us |  0.76 |    0.03 |    2 |
|   'Typin - 1 command' |  94.46 us | 1.974 us | 2.027 us |  1.00 |    0.00 |    3 |
|  'Typin - 2 commands' | 118.90 us | 2.668 us | 4.811 us |  1.29 |    0.05 |    4 |
|  'CliFx - 5 commands' | 126.71 us | 1.908 us | 1.692 us |  1.34 |    0.03 |    5 |
|  'Typin - 5 commands' | 180.16 us | 3.459 us | 3.701 us |  1.91 |    0.06 |    6 |
| 'CliFx - 10 commands' | 222.28 us | 3.079 us | 2.880 us |  2.35 |    0.07 |    7 |
| 'Typin - 10 commands' | 281.79 us | 4.679 us | 4.148 us |  2.99 |    0.06 |    8 |
| 'CliFx - 20 commands' | 454.07 us | 8.708 us | 8.942 us |  4.81 |    0.13 |    9 |
| 'Typin - 20 commands' | 519.70 us | 6.735 us | 6.300 us |  5.50 |    0.15 |   10 |

Legends:
  * Mean    : Arithmetic mean of all measurements
  * Error   : Half of 99.9% confidence interval
  * StdDev  : Standard deviation of all measurements
  * Ratio   : Mean of the ratio distribution ([Current]/[Baseline])
  * RatioSD : Standard deviation of the ratio distribution ([Current]/[Baseline])
  * Rank    : Relative position of current benchmark mean among all benchmarks (Arabic style)
  * 1 us    : 1 Microsecond (0.000001 sec)
