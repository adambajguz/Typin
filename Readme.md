# Typin

<p align="center">

[![Build](https://github.com/adambajguz/Typin/workflows/CI/badge.svg?branch=master)](https://github.com/adambajguz/Typin/actions)
[![Coverage](https://codecov.io/gh/adambajguz/Typin/branch/master/graph/badge.svg)](https://codecov.io/gh/adambajguz/Typin)
[![Version](https://img.shields.io/nuget/v/Typin.svg)](https://nuget.org/packages/Typin)
[![Downloads](https://img.shields.io/nuget/dt/Typin.svg)](https://nuget.org/packages/Typin)

</p>

<table>
 <tr>
    <td>
      <p align="center">
        <img src="https://raw.githubusercontent.com/adambajguz/Typin/master/.img/typin-logo-b-256px.png" width="75%">
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
  - [Etymology](#etymology)

 </tr>
</table>



**Typin** is a simple to use, yet powerful framework for building both interactive command line applications and command line tools. It is build based on the code of [CliFx](https://github.com/Tyrrrz/CliFx). Its primary goal is to completely take over the user input layer, letting you forget about the infrastructure and instead focus on writing your application. This framework uses a declarative class-first approach for defining commands, avoiding excessive boilerplate code and complex configurations.

An important property of Typin, when compared to some other libraries, is that it's not just a parser -- it's a complete application framework. The main goal of the library is to provide a consistent and enjoyable development experience when building command line applications. At its core, Typin is highly opinionated, giving preference to convention over configuration, strictness over extensibility, consistency over ambiguity, and so on.

## Features

- Complete application framework, not just an argument parser
- Requires minimal amount of code to get started
- Configuration via attributes
- Handles conversions to various types, including custom types
- Supports multi-level command hierarchies
- Supports interactive mode
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

Here's how Typin's execution overhead compares to that of other libraries.

```ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.14393.3443 (1607/AnniversaryUpdate/Redstone1)
Intel Core i5-4460 CPU 3.20GHz (Haswell), 1 CPU, 4 logical and 4 physical cores
Frequency=3124994 Hz, Resolution=320.0006 ns, Timer=TSC
.NET Core SDK=3.1.100
  [Host]     : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
  DefaultJob : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
```

|                               Method |        Mean |     Error |     StdDev | Ratio | RatioSD | Rank |
|------------------------------------- |------------:|----------:|-----------:|------:|--------:|-----:|
|                    CommandLineParser |    24.79 us |  0.166 us |   0.155 us |  0.49 |    0.00 |    1 |
|                                CliFx |    50.27 us |  0.248 us |   0.232 us |  1.00 |    0.00 |    2 |
|                               Typein |    50.27 us |  0.248 us |   0.232 us |  1.00 |    0.00 |    3 |
|                                Clipr |   160.22 us |  0.817 us |   0.764 us |  3.19 |    0.02 |    4 |
| McMaster.Extensions.CommandLineUtils |   166.45 us |  1.111 us |   1.039 us |  3.31 |    0.03 |    5 |
|                   System.CommandLine |   170.27 us |  0.599 us |   0.560 us |  3.39 |    0.02 |    6 |
|                            PowerArgs |   306.12 us |  1.495 us |   1.398 us |  6.09 |    0.03 |    7 |
|                               Cocona | 1,856.07 us | 48.727 us | 141.367 us | 37.88 |    2.60 |    8 |

## Etymology

Typin is made out of "Typ" for "Type" and "in" for "interactive". It's pronounced as "Typein".
