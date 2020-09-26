# Typin

<p align="center">

[![Build](https://github.com/adambajguz/Typin/workflows/CI/badge.svg?branch=master)](https://github.com/adambajguz/Typin/actions)
[![Coverage](https://codecov.io/gh/adambajguz/Typin/branch/master/graph/badge.svg?v=8)](https://codecov.io/gh/adambajguz/Typin)
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
  - [Features](#features)
  - [Installing Typin](#installing-typin)
  - [Documentation](#documentation)
  - [Screenshots](#screenshots)
  - [Benchmarks](#benchmarks)
  
  </td>
 </tr>
</table>



**Typin** is a simple to use, yet powerful framework for building both interactive command line applications and command line tools. 

> **Etymology:** Typin is made out of "Typ" for "Type" and "in" for "interactively". It's pronounced as "Type in".

It is build based on the code of [CliFx](https://github.com/Tyrrrz/CliFx). Its primary goal is to completely take over the user input layer, letting you forget about the infrastructure and instead focus on writing your application.
This framework uses a declarative class-first approach for defining commands, avoiding excessive boilerplate code and complex configurations.

Typin is not just a parser but a complete application framework. 
Its main goal is to provide an enjoyable, similar to ASP.NET Core, development experience when building command line applications.

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

## Documentation

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

|                               Method |         Mean |      Error |     StdDev | Ratio | RatioSD | Rank |
|------------------------------------- |-------------:|-----------:|-----------:|------:|--------:|-----:|
|                    CommandLineParser |     2.385 us |  0.0291 us |  0.0258 us |  0.03 |    0.00 |    1 |
|                                CliFx |    49.646 us |  0.2748 us |  0.2436 us |  0.64 |    0.00 |    2 |
|                                Typin |    77.431 us |  0.3132 us |  0.2930 us |  1.00 |    0.00 |    3 |
|                                Clipr |   126.747 us |  0.2351 us |  0.2084 us |  1.64 |    0.01 |    4 |
| McMaster.Extensions.CommandLineUtils |   130.208 us |  2.8264 us |  3.5745 us |  1.70 |    0.05 |    4 |
|                   System.CommandLine |   194.457 us |  0.4901 us |  0.4584 us |  2.51 |    0.01 |    5 |
|                            PowerArgs |   253.751 us |  0.9615 us |  0.8994 us |  3.28 |    0.02 |    6 |
|                               Cocona | 1,155.237 us | 23.0767 us | 30.0062 us | 15.09 |    0.41 |    7 |

|                Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|---------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|   'Typin - 1 command' |  77.30 us | 0.429 us | 0.380 us |  1.00 |    0.00 |    1 |
|  'Typin - 2 commands' |  98.45 us | 0.334 us | 0.296 us |  1.27 |    0.01 |    2 |
|  'Typin - 5 commands' | 162.13 us | 2.984 us | 2.791 us |  2.10 |    0.04 |    3 |
| 'Typin - 10 commands' | 260.74 us | 0.478 us | 0.424 us |  3.37 |    0.02 |    4 |
| 'Typin - 20 commands' | 500.15 us | 0.697 us | 0.582 us |  6.47 |    0.03 |    5 |

Legends:
  * Mean    : Arithmetic mean of all measurements
  * Error   : Half of 99.9% confidence interval
  * StdDev  : Standard deviation of all measurements
  * Ratio   : Mean of the ratio distribution ([Current]/[Baseline])
  * RatioSD : Standard deviation of the ratio distribution ([Current]/[Baseline])
  * Rank    : Relative position of current benchmark mean among all benchmarks (Arabic style)
  * 1 us    : 1 Microsecond (0.000001 sec)
